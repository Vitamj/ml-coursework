using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

public class Session
{
    public Session(SessionId id, SessionType sessionType, AccountId? accountId)
    {
        Id = id;
        SessionType = sessionType;
        AccountId = accountId;
        CreatedAt = System.DateTime.UtcNow;
    }

    public SessionId Id { get; }

    public SessionType SessionType { get; }

    public AccountId? AccountId { get; }

    public System.DateTime CreatedAt { get; }
}