using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.File;

public sealed class FileCopyCommand : ICommand
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
        if (!context.IsConnected || context.FileSystem is null) return new CommandResult.NotConnected();

        ResolvePathResult sourceResolved = context.ResolveVirtualPath(_source, mustBeDirectory: false);
        if (sourceResolved is ResolvePathResult.Failure sourceFail) return sourceFail.Error;

        ResolvePathResult destResolved = context.ResolveVirtualPath(_destinationDirectory, mustBeDirectory: true);
        if (destResolved is ResolvePathResult.Failure destFail) return destFail.Error;

        var s = (ResolvePathResult.Success)sourceResolved;
        var d = (ResolvePathResult.Success)destResolved;

        return context.FileSystem.CopyFile(s.Path, d.Path);
    }
}
