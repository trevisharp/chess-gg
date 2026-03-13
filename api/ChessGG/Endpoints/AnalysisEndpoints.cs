using Microsoft.AspNetCore.Mvc;

namespace ChessGG.Endpoints;

using Application.UseCases.GetAnalisys;

public static class AnalysisEndpoints
{
    public static IEndpointRouteBuilder MapAnalysisEndpoints(this IEndpointRouteBuilder route)
    {
        route.MapGet("/analysis/{player}", async (
            [FromServices]GetAnalisysUseCase useCase, string player) =>
        {
            var analysis = await useCase.RunAsync(new(player));

            if (analysis.Player is null)
                return Results.NotFound();

            return Results.Ok(new {
                analysis.Player,
                analysis.Analysis
            });
        });

        return route;
    }
}