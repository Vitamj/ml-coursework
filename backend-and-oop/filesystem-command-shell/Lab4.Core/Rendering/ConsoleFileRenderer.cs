namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;

public sealed class ConsoleFileRenderer : IFileRenderer
{
    public string Render(string content) => content ?? string.Empty;
}
