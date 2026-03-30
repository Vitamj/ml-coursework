namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.ValueObjects;

public readonly record struct Money(decimal Value)
{
    public static Money Zero => new(0);

    public static Money operator +(Money left, Money right) => new(left.Value + right.Value);

    public static Money operator -(Money left, Money right) => new(left.Value - right.Value);

    public static bool operator >(Money left, Money right) => left.Value > right.Value;

    public static bool operator <(Money left, Money right) => left.Value < right.Value;

    public static bool operator >=(Money left, Money right) => left.Value >= right.Value;

    public static bool operator <=(Money left, Money right) => left.Value <= right.Value;
}