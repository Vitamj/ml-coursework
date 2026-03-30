using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;

public sealed class ConnectCommand : ICommand
{
    private readonly IFileSystemFactory _factory;
    private readonly IPathParser _pathParser;

    public ConnectCommand(ConnectArgs args, IFileSystemFactory factory, IPathParser pathParser)
    {
        ArgumentNullException.ThrowIfNull(args);
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _pathParser = pathParser ?? throw new ArgumentNullException(nameof(pathParser));

        if (string.IsNullOrWhiteSpace(args.Address))
            throw new ArgumentException("Address cannot be empty.", nameof(args));

        if (string.IsNullOrWhiteSpace(args.Mode))
            throw new ArgumentException("Mode cannot be empty.", nameof(args));

        Args = args;
    }

    public ConnectArgs Args { get; }

    public CommandResult Execute(SessionContext context)
    {
        if (context is null) return new CommandResult.Failure("Context is null.");

        IFileSystem fileSystem = _factory.Create(Args.Mode);
        if (fileSystem is null)
            return new CommandResult.Failure("File system factory returned null.");

        IPath root;
        try
        {
            root = _pathParser.Parse(Args.Address).Normalize();
        }
        catch (Exception ex)
        {
            return new CommandResult.InvalidPath(ex.Message);
        }

        CommandResult result = context.Connect(fileSystem, root);
        if (result is CommandResult.Success)
            context.Output.WriteLine("Connected to " + root);

        return result;
    }
}
