namespace ChessGG.Application.UseCases.GetAnalisys;

using Application.Interfaces;

public class GetAnalisysUseCase(IAnalysisService service, IPlayerService playerService)
{
    public async Task<GetAnalisysResponse> RunAsync(GetAnalisysRequest request)
    {
        var player = await playerService.GetPlayerData(request.Player);
        if (player is null)
            return new(null, null);
        
        var analysis = await service.GetByPlayerAsync(request.Player);
        if (analysis is not null)
            return new(analysis, player);
        
        var empty = await service.CreateEmptyAsync(request.Player);
        return new(empty, player);
    }
}