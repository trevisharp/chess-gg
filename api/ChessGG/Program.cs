using Amazon;

using ChessGG.Application.Interfaces;
using ChessGG.Endpoints;
using ChessGG.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(provider => new DynamoDBClient(
    serviceUrl: builder.Configuration["DYNAMODB_URL"]
        ?? throw new Exception("missing 'DYNAMODB_URL' env."),
    region: RegionEndpoint.SAEast1,
    deploy: builder.Configuration["ENV"] == "prod"
));

builder.Services.AddTransient<IAnalisysService, DynamoDBAnalisysService>();
builder.Services.AddTransient<IRequestService, DynamoDBRequestService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();