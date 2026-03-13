using ChessGG.Application.UseCases.GetAnalisys;
using Microsoft.AspNetCore.Mvc;

namespace ChessGG.Endpoints;

public static class AnalysisEndpoints
{
    public static IEndpointRouteBuilder MapAnalysisEndpoints(this IEndpointRouteBuilder route)
    {
        route.MapGet("/analysis/{player}", async (
            [FromServices]GetAnalisysUseCase useCase, string player) =>
        {
            var analysis = await useCase.RunAsync(new(player));
            return analysis;
        });

        return route;
    }
}