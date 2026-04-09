using FluentValidation.AspNetCore;
using GenricRepository.Application;
using GenricRepository.Infrastructure;
using GenricRepository.Infrastructure.Persistence;
using GenricRepository.Presentation.Health;
using GenricRepository.Presentation.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<StartupStatus>();
builder.Services.AddHealthChecks()
    .AddCheck("live", () => HealthCheckResult.Healthy("Application is running."), tags: ["live"])
    .AddCheck<StartupReadinessHealthCheck>("startup", tags: ["ready"])
    .AddCheck<DatabaseHealthCheck>("database", tags: ["ready"]);

var app = builder.Build();
await RunStartupChecksAsync(app);

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live"),
    ResponseWriter = WriteHealthResponseAsync
});
app.MapHealthChecks("/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = WriteHealthResponseAsync
});
app.MapControllers();

app.Run();

static async Task RunStartupChecksAsync(WebApplication app)
{
    var startupStatus = app.Services.GetRequiredService<StartupStatus>();
    var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("StartupChecks");

    const int maxAttempts = 5;
    Exception? lastException = null;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (app.Environment.IsDevelopment())
            {
                await dbContext.Database.MigrateAsync();
            }

            if (!await dbContext.Database.CanConnectAsync())
            {
                throw new InvalidOperationException("Database connection failed during startup.");
            }

            startupStatus.MarkReady();
            logger.LogInformation("Startup checks completed successfully.");
            return;
        }
        catch (Exception exception)
        {
            lastException = exception;
            startupStatus.MarkFailed(exception.Message);
            logger.LogWarning(
                exception,
                "Startup check attempt {Attempt}/{MaxAttempts} failed.",
                attempt,
                maxAttempts);

            if (attempt < maxAttempts)
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)));
            }
        }
    }

    throw new InvalidOperationException("Startup checks failed after retries.", lastException);
}

static async Task WriteHealthResponseAsync(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";

    var payload = new
    {
        status = report.Status.ToString(),
        totalDuration = report.TotalDuration.TotalMilliseconds,
        checks = report.Entries.Select(entry => new
        {
            name = entry.Key,
            status = entry.Value.Status.ToString(),
            description = entry.Value.Description,
            duration = entry.Value.Duration.TotalMilliseconds,
            error = entry.Value.Exception?.Message
        })
    };

    await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
}
