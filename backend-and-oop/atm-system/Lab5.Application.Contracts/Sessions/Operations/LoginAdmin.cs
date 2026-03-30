namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions.Operations;

public static class LoginAdmin
{
    public record Request(string SystemPassword);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(System.Guid SessionKey) : Result;

        public sealed record InvalidCredentials : Result;
    }
}