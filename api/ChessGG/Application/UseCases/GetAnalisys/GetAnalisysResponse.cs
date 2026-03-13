using ChessGG.Domain;

namespace ChessGG.Application.UseCases.GetAnalisys;

public record GetAnalisysResponse(Analysis? Analysis, Player? Player);