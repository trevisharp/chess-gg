using ChessGG.Application.Interfaces;

namespace ChessGG.Application.UseCases.CreateRequest;

public class CreateRequestUseCase(IRequestService service)
{
    public async Task<CreateRequestResponse> RunAsync(CreateRequestRequest request)
    {
        if (request.PlayerName is "" or null)
            return new (false, "invalid player", Guid.Empty);
        
        var playerRequest = await service.GetByPlayerAsync(request.PlayerName);
        if (playerRequest is null)
        {
            playerRequest = await service.CreateAsync(request.PlayerName);
            // TODO: Send message
            return new (true, null, playerRequest.Id);
        }
        
        if (!playerRequest.CanRecreate())
            return new (true, null, playerRequest.Id);

        playerRequest.Recreate();
        // TODO: Send message
        await service.UpdateAsync(playerRequest);

        return new (true, null, playerRequest.Id);
    }
}