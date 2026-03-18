namespace ChessGG.Endpoints;

public static class Configure
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok());

        var api = app.MapGroup("/api");

        api.MapAnalysisEndpoints();
        api.MapRequestEndpoints();

        return api;
    }
}