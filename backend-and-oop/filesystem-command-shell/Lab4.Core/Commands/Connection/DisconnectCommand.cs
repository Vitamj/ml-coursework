using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;

public sealed class DisconnectCommand : ICommand
{
    public CommandResult Execute(SessionContext context)
    {
        if (context is null) return new CommandResult.Failure("Context is null.");

        CommandResult result = context.Disconnect();
        if (result is CommandResult.Success)
            context.Output.WriteLine("Disconnected");

        return result;
    }
}