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

        var analysis = 
            await analyses.GetByPlayerAsync(request.Player) ??
            await analyses.CreateEmptyAsync(request.Player);
        analysis.RequestId = req.Id.ToString();
        
        req.ProcessStatus = 0.25f;
        await requests.UpdateAsync(req);
        
        Thread.Sleep(500);
        analysis.FinalsAbility = Random.Shared.NextSingle();
        analysis.OpeningTheory = Random.Shared.NextSingle();
        analysis.TaticalAttention = Random.Shared.NextSingle();
        analysis.ThreatAvaliation = Random.Shared.NextSingle();
        analysis.TimeManagement = Random.Shared.NextSingle();

        await analyses.UpdateAsync(analysis);
        req.Status = RequestStatus.Completed;
        req.ProcessStatus = 1f;
        await requests.UpdateAsync(req);
        
        return new();
    }
}