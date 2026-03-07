namespace ChessGG.Domain;

public class Analysis
{
    public required Guid Id { get; init; }
    public required string Player { get; init; }
    public required string? RequestId { get; set; }
    public required float OpeningTeory { get; set; }
    public required float ThreatAvaliation { get; set; }
    public required float TaticalAttention { get; set; }
    public required float TimeManagement { get; set; }
    public required float FinalsAbility { get; set; }
}