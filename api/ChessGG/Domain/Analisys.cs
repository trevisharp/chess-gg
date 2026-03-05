namespace ChessGG.Domain;

public class Analisys
{
    public required string Player { get; init; }

    public required float OpeningTeory { get; set; }
    public required float ThreatAvaliation { get; set; }
    public required float TaticalAttention { get; set; }
    public required float TimeManagement { get; set; }
    public required float FinalsAbility { get; set; }
}