using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions.Operations;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions;

public interface ISessionService
{
    LoginUser.Result LoginUser(LoginUser.Request request);

    LoginAdmin.Result LoginAdmin(LoginAdmin.Request request);

    void Logout(System.Guid sessionKey);

    bool IsValidUserSession(System.Guid sessionKey);

    bool IsValidAdminSession(System.Guid sessionKey);

    long? GetAccountIdFromSession(System.Guid sessionKey);
}