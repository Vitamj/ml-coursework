using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public interface ICommand
{
    CommandResult Execute(SessionContext context);
}