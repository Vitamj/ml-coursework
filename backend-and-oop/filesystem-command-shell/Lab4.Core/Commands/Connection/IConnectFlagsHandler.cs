using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Connection;

public interface IConnectFlagsHandler
{
    ConnectArgs Handle(ITokenIterator iterator, ConnectArgs current);
    IConnectFlagsHandler AddNext(IConnectFlagsHandler next);
}