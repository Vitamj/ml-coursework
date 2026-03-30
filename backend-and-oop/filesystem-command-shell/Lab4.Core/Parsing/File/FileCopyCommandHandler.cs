using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.File;

public sealed class FileCopyCommandHandler : ICommandHandler
{
    private ICommandHandler? _next;

    public ICommand? Handle(ITokenIterator iterator, IPathParser pathParser)
    {
        if (iterator is null) throw new ArgumentNullException(nameof(iterator));
        if (pathParser is null) throw new ArgumentNullException(nameof(pathParser));

        if (!string.Equals(iterator.Current, "copy", StringComparison.OrdinalIgnoreCase))
            return _next?.Handle(iterator, pathParser);

        if (!iterator.MoveNext())
            return new ErrorCommand("file copy requires [SourcePath] [DestinationPath]");

        IPath sourcePath = ParsePathOrError(pathParser, iterator.Current, out ICommand? error1);
        if (error1 is not null) return error1;

        if (!iterator.MoveNext())
            return new ErrorCommand("file copy requires [SourcePath] [DestinationPath]");

        IPath destinationPath = ParsePathOrError(pathParser, iterator.Current, out ICommand? error2);
        if (error2 is not null) return error2;

        if (iterator.MoveNext())
            return new ErrorCommand($"Unexpected token '{iterator.Current}'");

        return new FileCopyCommand(sourcePath, destinationPath);
    }

    public ICommandHandler AddNext(ICommandHandler next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        return this;
    }

    private static IPath ParsePathOrError(IPathParser parser, string token, out ICommand? error)
    {
        try
        {
            error = null;
            return parser.Parse(token);
        }
        catch (Exception)
        {
            error = new ErrorCommand($"Invalid path: '{token}'");
            return parser.Parse("/");
        }
    }

    private sealed class FileCopyCommand : ICommand
    {
        private readonly IPath _source;
        private readonly IPath _destinationDirectory;

        public FileCopyCommand(IPath source, IPath destinationDirectory)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _destinationDirectory = destinationDirectory ?? throw new ArgumentNullException(nameof(destinationDirectory));
        }

        public CommandResult Execute(SessionContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (!context.IsConnected || context.FileSystem is null)
                return new CommandResult.NotConnected();

            ResolvePathResult srcResolved = context.ResolveVirtualPath(_source, mustBeDirectory: false);
            if (srcResolved is ResolvePathResult.Failure srcFail)
                return srcFail.Error;

            ResolvePathResult dstResolved = context.ResolveVirtualPath(_destinationDirectory, mustBeDirectory: true);
            if (dstResolved is ResolvePathResult.Failure dstFail)
                return dstFail.Error;

            var srcOk = (ResolvePathResult.Success)srcResolved;
            var dstOk = (ResolvePathResult.Success)dstResolved;


            return context.FileSystem.CopyFile(srcOk.Path, dstOk.Path);
        }
    }
}