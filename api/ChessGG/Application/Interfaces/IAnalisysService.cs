using ChessGG.Domain;

namespace ChessGG.Application.Interfaces;

public interface IAnalisysService
{
    Analisys GetByPlayer(string player);
}