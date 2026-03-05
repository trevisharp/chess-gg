using ChessGG.Domain;

namespace ChessGG.Application.Interfaces;

public interface IAnalisysService
{
    Task<Analisys?> GetByPlayerAsync(string player);
    Task CreateEmptyAsync(string player);
    Task UpdateAsync(Analisys analisys);
}