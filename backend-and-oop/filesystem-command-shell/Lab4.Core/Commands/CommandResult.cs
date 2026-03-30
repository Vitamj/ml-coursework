namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public abstract record CommandResult
{
    private CommandResult() { }

    public sealed record Success() : CommandResult;

    public sealed record Failure(string Message) : CommandResult;

    public sealed record NotConnected() : CommandResult;

    public sealed record FileNotFound(string Path) : CommandResult;

    public sealed record DirectoryNotFound(string Path) : CommandResult;

    public sealed record InvalidPath(string Path) : CommandResult;

    public sealed record NameCollision(string Path) : CommandResult;

    public sealed record FileContent(string Content) : CommandResult;
}