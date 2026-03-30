using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Tree;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Tree.Handlers;

public sealed class TreeListCommandHandler : CommandHandlerBase
{
    private readonly ITreeRenderer _renderer;

    public TreeListCommandHandler(ITreeRenderer renderer)
    {
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    public override ICommand? Handle(ITokenIterator iterator, IPathParser parser)
    {
        int start = iterator.Position;

        if (!string.Equals(iterator.Current, "list", StringComparison.Ordinal))
            return CallNext(iterator, parser, start);

        int depth = 1;

        while (iterator.MoveNext())
        {
            if (!string.Equals(iterator.Current, "-d", StringComparison.Ordinal))
                continue;

            if (!iterator.MoveNext())
                break;

            if (int.TryParse(iterator.Current, out int parsed) && parsed > 0)
                depth = parsed;
        }

        return new TreeListCommand(depth, _renderer);
    }
}
