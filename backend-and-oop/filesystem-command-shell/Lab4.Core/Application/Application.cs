using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Output;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Application;

public sealed class Application
{
    private readonly ICommandHandler _handler;
    private readonly IPathParser _pathParser;
    private readonly SessionContext _context;

    public Application(ICommandHandler handler, IPathParser pathParser, SessionContext context)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _pathParser = pathParser ?? throw new ArgumentNullException(nameof(pathParser));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Run()
    {
        IOutput output = _context.Output;

        while (true)
        {
            output.Write("> ");
            string? line = Console.ReadLine();
            if (line is null) break;

            if (string.Equals(line.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
                break;

            var iterator = new LineTokenIterator(line);
            if (!iterator.MoveNext()) continue;

            ICommand? command = _handler.Handle(iterator, _pathParser);
            if (command is null)
            {
                output.WriteLine("Unknown command.");
                continue;
            }

            CommandResult result = command.Execute(_context);
            PresentResult(result, output);
        }
    }

    private static void PresentResult(CommandResult result, IOutput output)
    {
        switch (result)
        {
            case CommandResult.Success:
                return;

            case CommandResult.NotConnected:
                output.WriteLine("Not connected.");
                return;

            case CommandResult.Failure fail:
                output.WriteLine("Error: " + fail.Message);
                return;

            case CommandResult.InvalidPath invalid:
                output.WriteLine("Invalid path: " + invalid.Path);
                return;

            case CommandResult.DirectoryNotFound dn:
                output.WriteLine("Directory not found: " + dn.Path);
                return;

            case CommandResult.FileNotFound fn:
                output.WriteLine("File not found: " + fn.Path);
                return;

            case CommandResult.NameCollision nc:
                output.WriteLine("Name collision: " + nc.Path);
                return;

            default:
                output.WriteLine("Unknown result.");
                return;
        }
    }
}
