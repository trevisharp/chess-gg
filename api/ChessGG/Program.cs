using Amazon;
using ChessGG.Endpoints;
using ChessGG.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(provider => new DynamoDBClient(
    serviceUrl: builder.Configuration["DYNAMODB_URL"]
        ?? throw new Exception("missing DYNAMODB_URL env."),
    region: RegionEndpoint.APSouth1,
    deploy: builder.Configuration["ENV"] == "dev"
));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();