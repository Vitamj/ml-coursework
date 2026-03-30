using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.File;

public sealed class FileShowCommandHandler : ICommandHandler
{
    private readonly IFileRenderer _renderer;
    private ICommandHandler? _next;

    public FileShowCommandHandler(IFileRenderer renderer)
    {
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    public ICommand? Handle(ITokenIterator iterator, IPathParser pathParser)
    {
        if (iterator is null) throw new ArgumentNullException(nameof(iterator));
        if (pathParser is null) throw new ArgumentNullException(nameof(pathParser));

        if (!string.Equals(iterator.Current, "show", StringComparison.OrdinalIgnoreCase))
            return _next?.Handle(iterator, pathParser);

        if (!iterator.MoveNext())
            return new ErrorCommand("file show requires [Path] -m console");


        IPath path;
        try
        {
            path = pathParser.Parse(iterator.Current);
        }
        catch (Exception)
        {
            return new ErrorCommand($"Invalid path: '{iterator.Current}'");
        }

        string? mode = null;

        while (iterator.MoveNext())
        {
            if (string.Equals(iterator.Current, "-m", StringComparison.OrdinalIgnoreCase))
            {
                if (!iterator.MoveNext())
                    return new ErrorCommand("file show requires value after -m");

                mode = iterator.Current;
                continue;
            }

            return new ErrorCommand($"Unexpected token '{iterator.Current}'");
        }

        if (string.IsNullOrWhiteSpace(mode))
            return new ErrorCommand("file show requires mandatory flag -m console");

        if (!string.Equals(mode, "console", StringComparison.OrdinalIgnoreCase))
            return new ErrorCommand("Only '-m console' is supported");

        return new FileShowCommand(path, _renderer);
    }

    public ICommandHandler AddNext(ICommandHandler next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        return this;
    }

    private sealed class FileShowCommand : ICommand
    {
        private readonly IPath _path;
        private readonly IFileRenderer _renderer;

        public FileShowCommand(IPath path, IFileRenderer renderer)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
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
            IPath absolute = ok.Path;

            if (!context.FileSystem.FileExists(absolute))
                return new CommandResult.FileNotFound(absolute.ToString() ?? string.Empty);

            CommandResult readResult = context.FileSystem.ReadFile(absolute);

            if (readResult is CommandResult.FileContent fileContent)
            {
                string rendered = _renderer.Render(fileContent.Content);
                context.Output.WriteLine(rendered);
                return new CommandResult.Success();
            }

            return readResult;
        }
    }
}
