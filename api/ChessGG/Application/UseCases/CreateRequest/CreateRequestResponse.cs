namespace ChessGG.Application.UseCases.CreateRequest;

public record CreateRequestResponse(
    bool Success,
    string? Reason,
    Guid CreatedId
);