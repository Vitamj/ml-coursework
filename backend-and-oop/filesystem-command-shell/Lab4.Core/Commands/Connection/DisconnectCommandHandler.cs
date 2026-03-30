using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Connection;

public sealed class DisconnectCommandHandler : CommandHandlerBase
{
    public override ICommand? Handle(ITokenIterator iterator, IPathParser pathParser)
    {
        int start = iterator.Position;

        if (!string.Equals(iterator.Current, "disconnect", StringComparison.Ordinal))
            return CallNext(iterator, pathParser, start);

        return new DisconnectCommand();
    }
}
