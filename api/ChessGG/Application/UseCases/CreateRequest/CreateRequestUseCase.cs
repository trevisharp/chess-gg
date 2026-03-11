namespace ChessGG.Application.UseCases.CreateRequest;

using Application.Interfaces;

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
        await service.UpdateAsync(playerRequest);
        await publisher.Publish("chess.analysis.exchange", "analysis", new {
            player = request.PlayerName
        });

        return new (true, null, playerRequest.Id);
    }
}