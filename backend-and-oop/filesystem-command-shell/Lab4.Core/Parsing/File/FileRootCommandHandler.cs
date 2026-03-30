using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.File;

public sealed class FileRootCommandHandler : ICommandHandler
{
    private readonly IReadOnlyCollection<ICommandHandler> _subHandlers;
    private ICommandHandler? _next;

    public FileRootCommandHandler(IReadOnlyCollection<ICommandHandler> subHandlers)
    {
        _subHandlers = subHandlers ?? throw new ArgumentNullException(nameof(subHandlers));
    }

    public ICommand? Handle(ITokenIterator iterator, IPathParser pathParser)
    {
        if (iterator is null) throw new ArgumentNullException(nameof(iterator));
        if (pathParser is null) throw new ArgumentNullException(nameof(pathParser));

        if (!string.Equals(iterator.Current, "file", StringComparison.OrdinalIgnoreCase))
            return _next?.Handle(iterator, pathParser);

        if (!iterator.MoveNext())
            return new ErrorCommand("file requires subcommand");

        foreach (ICommandHandler handler in _subHandlers)
        {
            int start = iterator.Position;

            ICommand? command = handler.Handle(iterator, pathParser);
            if (command is not null)
                return command;

            iterator.SetPosition(start);
        }

        return new ErrorCommand($"Unknown file subcommand: '{iterator.Current}'");
    }

    public ICommandHandler AddNext(ICommandHandler next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        return this;
    }
}
