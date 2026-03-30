using Itmo.ObjectOrientedProgramming.Lab5.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

public class Account
{
    public Account(AccountId id, string accountNumber, string pinCode, Money balance)
    {
        Id = id;
        AccountNumber = accountNumber;
        PinCode = pinCode;
        Balance = balance;
    }

    public AccountId Id { get; }

    public string AccountNumber { get; }

    public string PinCode { get; }

    public Money Balance { get; private set; }

    public bool Withdraw(Money amount)
    {
        if (amount.Value <= 0 || Balance < amount)
            return false;

        Balance -= amount;
        return true;
    }

    public void Deposit(Money amount)
    {
        if (amount.Value > 0)
            Balance += amount;
    }
}