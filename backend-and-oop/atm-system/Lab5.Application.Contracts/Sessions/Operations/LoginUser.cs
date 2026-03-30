namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions.Operations;

public static class LoginUser
{
    public record Request(string AccountNumber, string PinCode);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(System.Guid SessionKey) : Result;

        public sealed record AccountNotFound : Result;

        public sealed record InvalidCredentials : Result;
    }
}