using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Tree;

public sealed class TreeGotoCommand : ICommand
{
    public TreeGotoCommand(IPath path)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public IPath Path { get; }

    public CommandResult Execute(SessionContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        return context.ChangeDirectory(Path);
    }
}
