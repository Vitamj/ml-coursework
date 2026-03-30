using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Output;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.File;

public sealed class FileShowCommand : ICommand
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
        if (!context.IsConnected || context.FileSystem is null) return new CommandResult.NotConnected();

        ResolvePathResult resolved = context.ResolveVirtualPath(_path, mustBeDirectory: false);
        if (resolved is ResolvePathResult.Failure fail) return fail.Error;

        var ok = (ResolvePathResult.Success)resolved;

        CommandResult readResult = context.FileSystem.ReadFile(ok.Path);
        if (readResult is not CommandResult.FileContent content) return readResult;

        string rendered = _renderer.Render(content.Content);
        context.Output.WriteLine(rendered);

        return new CommandResult.Success();
    }
}