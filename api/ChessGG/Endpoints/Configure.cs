namespace ChessGG.Endpoints;

public static class Configure
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        api.MapAnalysisEndpoints();
        api.MapRequestEndpoints();

        return api;
    }
}