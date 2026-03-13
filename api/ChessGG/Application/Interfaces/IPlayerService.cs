using ChessGG.Domain;

namespace ChessGG.Application.Interfaces;

public interface IPlayerService
{
    Task<Player?> GetPlayerData(string playerName);
}