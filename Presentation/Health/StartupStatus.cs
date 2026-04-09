namespace GenricRepository.Presentation.Health;

public sealed class StartupStatus
{
    public bool IsReady { get; private set; }
    public string? FailureReason { get; private set; }

    public void MarkReady()
    {
        IsReady = true;
        FailureReason = null;
    }

    public void MarkFailed(string reason)
    {
        IsReady = false;
        FailureReason = reason;
    }
}
