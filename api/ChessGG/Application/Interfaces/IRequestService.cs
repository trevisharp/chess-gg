namespace ChessGG.Application.Interfaces;

using Domain;

public interface IRequestService
{
    Task<Request?> GetByIdAsync(string id);
    Task<Request?> CreateAsync(string player);
    Task UpdateAsync(Request request);
}