namespace ChessGG.Domain;

public class Request
{
    public required Guid Id { get; init; }
    public required string Player { get; init; }
    public required DateTime Creation { get; set; }
    public required RequestStatus Status { get; set; }
    public required float ProcessStatus { get; set; }

    public bool CanRecreate()
    {
        var passed = DateTime.UtcNow - Creation;
        return passed.TotalMinutes > 4;
    }

    public Request Recreate()
    {
        Creation = DateTime.UtcNow;
        Status = RequestStatus.Waiting;
        ProcessStatus = 0;
        return this;
    }
}