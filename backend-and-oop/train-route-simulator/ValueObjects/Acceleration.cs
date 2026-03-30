namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed record Acceleration
{
    public double MetersPerSecondSquared { get; }

    public Acceleration(double metersPerSecondSquared)
    {
        MetersPerSecondSquared = metersPerSecondSquared;
    }

    public static Acceleration FromValue(double value) => new(value);

    public static Speed operator *(Acceleration left, Time right) => 
        Speed.FromMetersPerSecond(left.MetersPerSecondSquared * right.Seconds);
    
    public static bool operator ==(Acceleration left, double right) => left.MetersPerSecondSquared == right;
    public static bool operator !=(Acceleration left, double right) => left.MetersPerSecondSquared != right;
}