namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts.Operations;

public static class Withdraw
{
    public record Request(decimal Amount);

    public abstract record Result
    {
        private Result() { }

        public sealed record Success(decimal NewBalance) : Result;

        public sealed record InsufficientFunds : Result;

        public sealed record InvalidAmount : Result;

        public sealed record AccountNotFound : Result;

        public sealed record Unauthorized : Result;
    }
}