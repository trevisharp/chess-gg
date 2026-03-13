namespace ChessGG.Infrastructure;

using Domain;
using Application.Interfaces;

public class ChessPlayerService(ChessService service) : IPlayerService
{
    record ChessPlayer(
        string Avatar,
        string Url,
        string Username
    );

    public async Task<Player?> GetPlayerData(string playerName)
    {
        var data = await service.Get<ChessPlayer>(playerName);
        if (data is null)
            return null;
        
        return new Player {
            AvatarUrl = data.Avatar,
            Url = data.Url,
            Username = data.Username
        };
    }
}