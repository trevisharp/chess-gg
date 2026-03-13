namespace ChessGG.Application.UseCases.GenerateAnalisys;

using Domain;
using Application.Interfaces;

public class GenerateAnalisysUseCase(
    IAnalysisService analyses,
    IRequestService requests
)
{
    public async Task<GenerateAnalisysResponse> RunAsync(GenerateAnalisysRequest request)
    {
        var req = await requests.GetByPlayerAsync(request.Player)
            ?? throw new Exception("Request has not yet been created.");

        req.Status = RequestStatus.InProcess;
        req.ProcessStatus = 0.1f;
        await requests.UpdateAsync(req);

        var analysis = await analyses.GetByPlayerAsync(request.Player);

        throw new NotImplementedException();
    }
}