namespace ChessGG.Endpoints;

public static class AnalysisEndpoints
{
    public static IEndpointRouteBuilder MapAnalysisEndpoints(this IEndpointRouteBuilder route)
    {
        route.MapGet("/analysis/{player}", (string player) =>
        {
            
        });

        return route;
    }
}