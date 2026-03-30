namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed record Mass
{
    public double Kilograms { get; }

    public Mass(double kilograms)
    {
        if (kilograms <= 0)
            throw new ArgumentException("Mass must be positive", nameof(kilograms));
        Kilograms = kilograms;
    }

    public static Mass FromKilograms(double kg) => new(kg);
}