using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Connection;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.File;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing.Tree.Handlers;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;

public sealed class CommandHandlerChainFactory
{
    private readonly IFileSystemFactory _fileSystemFactory;
    private readonly ITreeRenderer _treeRenderer;
    private readonly IFileRenderer _fileRenderer;

    public CommandHandlerChainFactory(
        IFileSystemFactory fileSystemFactory,
        ITreeRenderer treeRenderer,
        IFileRenderer fileRenderer)
    {
        _fileSystemFactory = fileSystemFactory ?? throw new ArgumentNullException(nameof(fileSystemFactory));
        _treeRenderer = treeRenderer ?? throw new ArgumentNullException(nameof(treeRenderer));
        _fileRenderer = fileRenderer ?? throw new ArgumentNullException(nameof(fileRenderer));
    }

    public ICommandHandler Create()
    {

        IConnectFlagsHandler connectFlags = new ConnectModeFlagHandler();
        var connect = new ConnectCommandHandler(connectFlags, _fileSystemFactory);
        var disconnect = new DisconnectCommandHandler();

        var treeGoto = new TreeGotoCommandHandler();
        var treeList = new TreeListCommandHandler(_treeRenderer);
        treeGoto.AddNext(treeList);

        var treeRoot = new TreeRootCommandHandler(treeGoto);

        var fileShow = new FileShowCommandHandler(_fileRenderer);
        var fileMove = new FileMoveCommandHandler();
        var fileCopy = new FileCopyCommandHandler();
        var fileDelete = new FileDeleteCommandHandler();
        var fileRename = new FileRenameCommandHandler();

        var fileRoot = new FileRootCommandHandler(new ICommandHandler[]
        {
            fileShow,
            fileMove,
            fileCopy,
            fileDelete,
            fileRename,
        });

        connect.AddNext(disconnect);
        disconnect.AddNext(treeRoot);
        treeRoot.AddNext(fileRoot);

        return connect;
    }
}
