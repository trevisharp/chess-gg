namespace ChessGG.Endpoints;

public static class RequestEndpoints
{
    public static IEndpointRouteBuilder MapRequestEndpoints(this IEndpointRouteBuilder route)
    {
        route.MapGet("/request/{id}", (string id) =>
        {
            
        });

        route.MapPost("/request", () =>
        {
            
        });

        return route;
    }
}