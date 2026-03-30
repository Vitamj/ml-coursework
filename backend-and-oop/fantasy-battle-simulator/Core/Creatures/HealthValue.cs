using System;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public record HealthValue
{
    public int Value { get; }

    public HealthValue(int value)
    {
        if (value < 0)
            throw new ArgumentException("Health value cannot be negative", nameof(value));

        Value = value;
    }

    public static implicit operator int(HealthValue health) => health.Value;
    public static implicit operator HealthValue(int value) => new(value);
}