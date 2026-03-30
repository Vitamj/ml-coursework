using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Connection;

public sealed class ConnectCommandHandler : CommandHandlerBase
{
    private readonly IConnectFlagsHandler? _flags;
    private readonly IFileSystemFactory _fileSystemFactory;

    public ConnectCommandHandler(IConnectFlagsHandler? flags, IFileSystemFactory fileSystemFactory)
    {
        _flags = flags;
        _fileSystemFactory = fileSystemFactory ?? throw new ArgumentNullException(nameof(fileSystemFactory));
    }

    public override ICommand? Handle(ITokenIterator iterator, IPathParser pathParser)
    {
        int start = iterator.Position;

        if (!string.Equals(iterator.Current, "connect", StringComparison.Ordinal))
            return CallNext(iterator, pathParser, start);

        if (!iterator.MoveNext())
            return null;

        string address = iterator.Current;
        if (string.IsNullOrWhiteSpace(address))
            return null;

        ConnectArgs args = ConnectArgs.CreateDefault(address);

        while (iterator.MoveNext())
        {
            if (_flags is null) continue;
            args = _flags.Handle(iterator, args);
        }

        return new ConnectCommand(args, _fileSystemFactory, pathParser);
    }
}
