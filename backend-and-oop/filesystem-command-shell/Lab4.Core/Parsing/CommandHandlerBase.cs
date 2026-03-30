using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing;

public abstract class CommandHandlerBase : ICommandHandler
{
    private ICommandHandler? _next;

    public ICommandHandler AddNext(ICommandHandler next)
    {
        _next ??= next;
        return next;
    }

    public abstract ICommand? Handle(ITokenIterator iterator, IPathParser pathParser);

    protected ICommand? CallNext(ITokenIterator iterator, IPathParser pathParser, int restorePosition)
    {
        if (_next is null) return null;

        iterator.SetPosition(restorePosition);
        return _next.Handle(iterator, pathParser);
    }
}
