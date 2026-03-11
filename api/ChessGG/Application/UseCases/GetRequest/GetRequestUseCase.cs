using ChessGG.Application.Interfaces;

namespace ChessGG.Application.UseCases.GetRequest;

public class GetRequestUseCase(IRequestService service)
{
    public async Task<GetRequestResponse> RunAsync(GetRequestRequest request)
    {
        var obj = await service.GetByPlayerAsync(request.Player);
        return new(obj);
    }
}