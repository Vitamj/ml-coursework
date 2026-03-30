using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Persistence.Queries;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions.Operations;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Services;

public class SessionService : ISessionService
{
    private readonly IPersistenceContext _context;
    private readonly string _systemPassword;

    public SessionService(IPersistenceContext context, string systemPassword)
    {
        _context = context;
        _systemPassword = systemPassword;
    }

    public LoginUser.Result LoginUser(LoginUser.Request request)
    {
        Account? account = _context.Accounts.Query(new AccountQuery(AccountNumber: request.AccountNumber));

        if (account is null)
            return new LoginUser.Result.AccountNotFound();

        if (!string.Equals(account.PinCode, request.PinCode, System.StringComparison.Ordinal))
            return new LoginUser.Result.InvalidCredentials();

        var sessionId = new SessionId(System.Guid.NewGuid());
        var session = new Session(sessionId, SessionType.User, account.Id);
        _context.Sessions.Add(session);

        return new LoginUser.Result.Success(sessionId.Value);
    }

    public LoginAdmin.Result LoginAdmin(LoginAdmin.Request request)
    {
        if (!string.Equals(_systemPassword, request.SystemPassword, System.StringComparison.Ordinal))
            return new LoginAdmin.Result.InvalidCredentials();

        var sessionId = new SessionId(System.Guid.NewGuid());
        var session = new Session(sessionId, SessionType.Admin, null);
        _context.Sessions.Add(session);

        return new LoginAdmin.Result.Success(sessionId.Value);
    }

    public void Logout(System.Guid sessionKey)
    {
        _context.Sessions.Remove(new SessionId(sessionKey));
    }

    public bool IsValidUserSession(System.Guid sessionKey)
    {
        Session? session = _context.Sessions.Query(new SessionQuery(Id: new SessionId(sessionKey)));
        return session is { SessionType: SessionType.User };
    }

    public bool IsValidAdminSession(System.Guid sessionKey)
    {
        Session? session = _context.Sessions.Query(new SessionQuery(Id: new SessionId(sessionKey)));
        return session is { SessionType: SessionType.Admin };
    }

    public long? GetAccountIdFromSession(System.Guid sessionKey)
    {
        Session? session = _context.Sessions.Query(new SessionQuery(Id: new SessionId(sessionKey)));
        return session?.AccountId?.Value;
    }
}
