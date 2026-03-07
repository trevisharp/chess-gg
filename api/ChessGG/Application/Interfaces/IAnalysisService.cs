using ChessGG.Domain;

namespace ChessGG.Application.Interfaces;

public interface IAnalysisService
{
    Task<Analysis?> GetByPlayerAsync(string player);
    Task CreateEmptyAsync(string player);
    Task UpdateAsync(Analysis analysis);
}