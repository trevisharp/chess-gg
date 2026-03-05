namespace ChessGG.Application.Interfaces;

using Domain;

public interface IRequestService
{
    Request GetById(string id);
    Request Create(string player);
}