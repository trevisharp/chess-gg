using Microsoft.AspNetCore.Mvc;

namespace ChessGG.Endpoints;

using Contracts;
using Application.UseCases.CreateRequest;

public static class RequestEndpoints
{
    public static IEndpointRouteBuilder MapRequestEndpoints(this IEndpointRouteBuilder route)
    {
        route.MapGet("/request/{id}", (string id) =>
        {
            return Results.Ok();
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