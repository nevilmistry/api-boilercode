namespace GenricRepository.Application.Contracts.Common;

public sealed record ApiEnvelope<T>(
    bool Success,
    int StatusCode,
    string Message,
    T? Data);

public static class ApiEnvelope
{
    public static ApiEnvelope<T> Ok<T>(T data, string message = "Request successful.")
        => new(true, 200, message, data);

    public static ApiEnvelope<T> Created<T>(T data, string message = "Created successfully.")
        => new(true, 201, message, data);

    public static ApiEnvelope<object?> SuccessMessage(string message = "Operation successful.")
        => new(true, 200, message, null);

    public static ApiEnvelope<object?> NotFound(string message = "Resource not found.")
        => new(false, 404, message, null);
}
