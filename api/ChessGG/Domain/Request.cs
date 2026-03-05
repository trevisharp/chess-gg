namespace ChessGG.Domain;

public class Request
{
    public required Guid Id { get; init; }
    public required string Player { get; init; }
    public required DateTime Creation { get; init; }
    public required RequestStatus Status { get; set; }
    public required float ProcessStatus { get; set; }
}