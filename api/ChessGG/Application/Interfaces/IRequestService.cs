namespace ChessGG.Application.Interfaces;

using Domain;

public interface IRequestService
{
    Task<Request?> GetByPlayerAsync(string player);
    Task<Request> CreateAsync(string player);
    Task UpdateAsync(Request request);
}