namespace ChessGG.Domain;

public enum RequestStatus
{
    Waiting,
    Completed,
    WaitingRetry,
    InProcess,
    Failed
}