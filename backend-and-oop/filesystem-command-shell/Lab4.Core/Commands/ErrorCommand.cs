using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public sealed class ErrorCommand : ICommand
{
    private readonly string _message;

    public ErrorCommand(string message)
    {
        _message = string.IsNullOrWhiteSpace(message)
            ? "Invalid command."
            : message;
    }

    public CommandResult Execute(SessionContext context)
    {
        return new CommandResult.Failure(_message);
    }
}
