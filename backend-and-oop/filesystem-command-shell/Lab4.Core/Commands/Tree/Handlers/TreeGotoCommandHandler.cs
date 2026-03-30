using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Tree;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Tree.Handlers;

public sealed class TreeGotoCommandHandler : CommandHandlerBase
{
    public override ICommand? Handle(ITokenIterator iterator, IPathParser parser)
    {
        int start = iterator.Position;

        if (!string.Equals(iterator.Current, "goto", StringComparison.Ordinal))
            return CallNext(iterator, parser, start);

        if (!iterator.MoveNext())
            return null;

        IPath path = parser.Parse(iterator.Current);
        return new TreeGotoCommand(path);
    }
}
