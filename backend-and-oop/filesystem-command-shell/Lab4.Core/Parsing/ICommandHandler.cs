using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing;

public interface ICommandHandler
{
    ICommand? Handle(ITokenIterator iterator, IPathParser pathParser);
    ICommandHandler AddNext(ICommandHandler next);
}
