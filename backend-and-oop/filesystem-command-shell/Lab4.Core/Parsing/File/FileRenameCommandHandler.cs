using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.File;

public sealed class FileRenameCommandHandler : ICommandHandler
{
    private ICommandHandler? _next;

    public ICommand? Handle(ITokenIterator iterator, IPathParser pathParser)
    {
        if (iterator is null) throw new ArgumentNullException(nameof(iterator));
        if (pathParser is null) throw new ArgumentNullException(nameof(pathParser));

        if (!string.Equals(iterator.Current, "rename", StringComparison.OrdinalIgnoreCase))
            return _next?.Handle(iterator, pathParser);

        if (!iterator.MoveNext())
            return new ErrorCommand("file rename requires [Path] [Name]");

        IPath path;
        try
        {
            path = pathParser.Parse(iterator.Current);
        }
        catch (Exception)
        {
            return new ErrorCommand($"Invalid path: '{iterator.Current}'");
        }

        if (!iterator.MoveNext())
            return new ErrorCommand("file rename requires [Path] [Name]");

        string newName = iterator.Current;

        if (string.IsNullOrWhiteSpace(newName))
            return new ErrorCommand("file rename requires non-empty [Name]");

        if (newName.Contains('/', StringComparison.Ordinal) || newName.Contains('\\', StringComparison.Ordinal))
            return new ErrorCommand("Name must not be a path");

        if (iterator.MoveNext())
            return new ErrorCommand($"Unexpected token '{iterator.Current}'");

        return new FileRenameCommand(path, newName);
    }

    public ICommandHandler AddNext(ICommandHandler next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        return this;
    }

    private sealed class FileRenameCommand : ICommand
    {
        private readonly IPath _path;
        private readonly string _newName;

        public FileRenameCommand(IPath path, string newName)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _newName = newName ?? throw new ArgumentNullException(nameof(newName));
        }

        public CommandResult Execute(SessionContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (!context.IsConnected || context.FileSystem is null)
                return new CommandResult.NotConnected();

            ResolvePathResult resolved = context.ResolveVirtualPath(_path, mustBeDirectory: false);
            if (resolved is ResolvePathResult.Failure fail)
                return fail.Error;

            var ok = (ResolvePathResult.Success)resolved;

            if (!context.FileSystem.FileExists(ok.Path))
                return new CommandResult.FileNotFound(ok.Path.ToString() ?? string.Empty);

            return context.FileSystem.RenameFile(ok.Path, _newName);
        }
    }
}
