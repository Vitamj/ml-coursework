namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;

public sealed record ConnectArgs(string Address, string Mode)
{
    public static ConnectArgs CreateDefault(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.", nameof(address));

        return new ConnectArgs(address, "local");
    }
}
