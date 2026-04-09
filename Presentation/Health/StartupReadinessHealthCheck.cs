using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GenricRepository.Presentation.Health;

public sealed class StartupReadinessHealthCheck : IHealthCheck
{
    private readonly StartupStatus _startupStatus;

    public StartupReadinessHealthCheck(StartupStatus startupStatus)
    {
        _startupStatus = startupStatus;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        if (_startupStatus.IsReady)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Startup checks completed."));
        }

        var reason = _startupStatus.FailureReason ?? "Startup checks are still running.";
        return Task.FromResult(HealthCheckResult.Unhealthy(reason));
    }
}
