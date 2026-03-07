using ChessGG.Application.Interfaces;

namespace ChessGG.Application.UseCases.CreateRequest;

public class CreateRequestUseCase(IRequestService service, IPublisher publisher)
{
    public async Task<CreateRequestResponse> RunAsync(CreateRequestRequest request)
    {
        if (request.PlayerName is "" or null)
            return new (false, "invalid player", Guid.Empty);
        
        var playerRequest = await service.GetByPlayerAsync(request.PlayerName);
        if (playerRequest is null)
        {
            playerRequest = await service.CreateAsync(request.PlayerName);
            await publisher.Publish("chess.analysis.exchange", "analysis", new {
                player = request.PlayerName
            });
            return new (true, null, playerRequest.Id);
        }
        
        if (!playerRequest.CanRecreate())
            return new (true, null, playerRequest.Id);

        playerRequest.Recreate();
        await publisher.Publish("chess.analysis.exchange", "analysis", new {
            player = request.PlayerName
        });
        await service.UpdateAsync(playerRequest);

        return new (true, null, playerRequest.Id);
    }
}