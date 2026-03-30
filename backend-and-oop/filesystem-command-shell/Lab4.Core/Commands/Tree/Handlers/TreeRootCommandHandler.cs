using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Tree.Handlers;

public sealed class TreeRootCommandHandler : CommandHandlerBase
{
    private readonly ICommandHandler _subChain;

    public TreeRootCommandHandler(ICommandHandler subChain)
    {
        _subChain = subChain ?? throw new ArgumentNullException(nameof(subChain));
    }

    public override ICommand? Handle(ITokenIterator iterator, IPathParser parser)
    {
        int start = iterator.Position;

        if (!string.Equals(iterator.Current, "tree", StringComparison.Ordinal))
            return CallNext(iterator, parser, start);

        if (!iterator.MoveNext())
            return null;

        return _subChain.Handle(iterator, parser);
    }
}
