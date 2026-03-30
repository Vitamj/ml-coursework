namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts.Operations;

public static class GetBalance
{
    public abstract record Result
    {
        private Result() { }

        public sealed record Success(decimal Balance) : Result;

        public sealed record AccountNotFound : Result;

        public sealed record Unauthorized : Result;
    }
}