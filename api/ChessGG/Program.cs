using ChessGG.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();