namespace ChessGG.Application.UseCases.GetAnalisys;

using Application.Interfaces;

public class GetAnalisysUseCase(IAnalysisService service)
{
    public async Task<GetAnalisysResponse> RunAsync(GetAnalisysRequest request)
    {
        var analysis = await service.GetByPlayerAsync(request.Player);
        if (analysis is not null)
            return new(analysis);
        
        var empty = await service.CreateEmptyAsync(request.Player);
        return new(empty);  
    }
}