using System;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public record AttackValue
{
    public int Value { get; }

    public AttackValue(int value)
    {
        if (value < 0)
            throw new ArgumentException("Attack value cannot be negative", nameof(value));

        Value = value;
    }

    public static implicit operator int(AttackValue attack) => attack.Value;
    public static implicit operator AttackValue(int value) => new(value);
}