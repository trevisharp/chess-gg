using Microsoft.AspNetCore.Mvc;

namespace ChessGG.Endpoints;

using Contracts;
using Application.UseCases.CreateRequest;
using Application.UseCases.GetRequest;

public static class RequestEndpoints
{
    public static IEndpointRouteBuilder MapRequestEndpoints(this IEndpointRouteBuilder route)
    {
        route.MapGet("/request/{player}", async (string player,
            [FromServices]GetRequestUseCase useCase) =>
        {
            var response = await useCase.RunAsync(new(player));

            if (response.Request is null)
                return Results.NotFound();

            return Results.Ok(new {
                response.Request.Player,
                response.Request.ProcessStatus,
                response.Request.Creation,
                response.Request.Status
            });
        });

        route.MapPost("/request", async (
            [FromServices]CreateRequestUseCase useCase,
            [FromBody]CreateRequestPayload payload) =>
        {
            var response = await useCase.RunAsync(new(payload.Player));

            if (!response.Success && response.CreatedId == Guid.Empty)
                return Results.BadRequest(response.Reason);
            
            return Results.Ok(new { response.CreatedId });
        });

        return route;
    }
}