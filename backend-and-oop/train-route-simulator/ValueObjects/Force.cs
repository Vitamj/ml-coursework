namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed record Force
{
    public double Newtons { get; }

    public Force(double newtons)
    {
        Newtons = newtons;
    }

    public static Acceleration operator /(Force left, Mass right) => 
        Acceleration.FromMetersPerSecondSquared(left.Newtons / right.Kilograms);
}