using Itmo.ObjectOrientedProgramming.Lab4.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.Connection;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Output;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public static class Program
{
    public static void Main()
    {
        IOutput output = new ConsoleOutput();
        IPathParser pathParser = new UnixPathParser();

        var context = new SessionContext(output, pathParser);

        IFileSystemFactory fileSystemFactory = new LocalFileSystemFactory();

        ITreeRenderer treeRenderer = new TreeRenderer(pathParser, "[D]", "[F]", "  ");
        IFileRenderer fileRenderer = new ConsoleFileRenderer();

        var chainFactory = new CommandHandlerChainFactory(fileSystemFactory, treeRenderer, fileRenderer);
        ICommandHandler handlerChain = chainFactory.Create();

        var app = new Application(handlerChain, pathParser, context);
        app.Run();
    }
}
