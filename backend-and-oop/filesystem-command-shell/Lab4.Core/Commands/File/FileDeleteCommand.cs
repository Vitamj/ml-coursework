using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.File;

public sealed class FileDeleteCommand : ICommand
{
    private readonly IPath _path;

    public FileDeleteCommand(IPath path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public CommandResult Execute(SessionContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (!context.IsConnected || context.FileSystem is null) return new CommandResult.NotConnected();

        ResolvePathResult resolved = context.ResolveVirtualPath(_path, mustBeDirectory: false);
        if (resolved is ResolvePathResult.Failure fail) return fail.Error;

        var ok = (ResolvePathResult.Success)resolved;
        return context.FileSystem.DeleteFile(ok.Path);
    }
}
