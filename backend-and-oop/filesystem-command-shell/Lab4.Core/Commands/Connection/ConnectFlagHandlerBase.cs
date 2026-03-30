using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Connection;

public abstract class ConnectFlagHandlerBase : IConnectFlagsHandler
{
    private IConnectFlagsHandler? _next;

    public IConnectFlagsHandler AddNext(IConnectFlagsHandler next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        return next;
    }

    public abstract ConnectArgs Handle(ITokenIterator iterator, ConnectArgs current);

    protected ConnectArgs CallNext(ITokenIterator iterator, ConnectArgs current)
        => _next is null ? current : _next.Handle(iterator, current);
}