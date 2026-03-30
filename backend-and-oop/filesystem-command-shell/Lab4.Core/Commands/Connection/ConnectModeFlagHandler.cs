using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Connection;

public sealed class ConnectModeFlagHandler : ConnectFlagHandlerBase
{
    public override ConnectArgs Handle(ITokenIterator iterator, ConnectArgs current)
    {
        if (!string.Equals(iterator.Current, "-m", StringComparison.Ordinal))
            return CallNext(iterator, current);

        if (!iterator.MoveNext())
            return current;

        string mode = iterator.Current;
        if (string.IsNullOrWhiteSpace(mode))
            return current;

        return current with { Mode = mode };
    }
}
