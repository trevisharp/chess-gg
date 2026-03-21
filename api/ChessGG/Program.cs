using ChessGG.Application.Interfaces;
using ChessGG.Application.UseCases.CreateRequest;
using ChessGG.Application.UseCases.GenerateAnalisys;
using ChessGG.Application.UseCases.GetAnalisys;
using ChessGG.Application.UseCases.GetRequest;
using ChessGG.Endpoints;
using ChessGG.Endpoints.Consumers;
using ChessGG.Infrastructure;
using ChessGG.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(provider => 
    builder.Configuration["ENV"] == "prod" ?
        new DynamoDBClient() :

        new DynamoDBClient(
            serviceUrl: builder.Configuration["DYNAMODB_URL"]
                ?? throw new Exception("missing 'DYNAMODB_URL' env."),
            region: "sa-east-1",
            accessKey: builder.Configuration["AWS_SECRET_ACCESS_KEY"]
                ?? throw new Exception("missing 'AWS_SECRET_ACCESS_KEY' env."),
            secretKey: builder.Configuration["AWS_ACCESS_KEY_ID"]
                ?? throw new Exception("missing 'AWS_ACCESS_KEY_ID' env.")
        )
);

builder.Services.ConfigureMessaging(builder.Configuration);

builder.Services.AddTransient<GetRequestUseCase>();
builder.Services.AddTransient<CreateRequestUseCase>();
builder.Services.AddTransient<GenerateAnalisysUseCase>();
builder.Services.AddTransient<GetAnalisysUseCase>();

builder.Services.AddTransient<ChessService>();
builder.Services.AddTransient<IPlayerService, ChessPlayerService>();

builder.Services.AddTransient<IAnalysisService, DynamoDBAnalysisService>();
builder.Services.AddTransient<IRequestService, DynamoDBRequestService>();

builder.Services.AddHostedService<AnalysisConsumer>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapEndpoints();

await app.UseMessaging();

app.Run();