namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts.Operations;

public static class CreateAccount
{
    public record Request(string AccountNumber, string PinCode);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(long AccountId) : Result;

        public sealed record AccountAlreadyExists : Result;

        public sealed record Unauthorized : Result;
    }
}