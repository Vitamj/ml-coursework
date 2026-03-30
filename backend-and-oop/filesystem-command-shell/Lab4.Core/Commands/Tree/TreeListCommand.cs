using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Tree;

public sealed class TreeListCommand : ICommand
{
    private readonly ITreeRenderer _renderer;

    public TreeListCommand(int depth, ITreeRenderer renderer)
    {
        if (depth < 1) throw new ArgumentOutOfRangeException(nameof(depth));

        Depth = depth;
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    public int Depth { get; }

    public CommandResult Execute(SessionContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        if (!context.IsConnected || context.FileSystem is null)
            return new CommandResult.NotConnected();

        _renderer.RenderTree(context.FileSystem, context.LocalPath, Depth, context.Output);
        return new CommandResult.Success();
    }
}
