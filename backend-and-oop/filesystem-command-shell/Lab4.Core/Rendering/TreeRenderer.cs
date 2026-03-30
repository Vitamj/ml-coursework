using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Output;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;

public sealed class TreeRenderer : ITreeRenderer
{
    private readonly IPathParser _pathParser;
    private readonly string _directoryPrefix;
    private readonly string _filePrefix;
    private readonly string _indentUnit;

    public TreeRenderer(
        IPathParser pathParser,
        string directoryPrefix,
        string filePrefix,
        string indentUnit)
    {
        _pathParser = pathParser ?? throw new ArgumentNullException(nameof(pathParser));
        _directoryPrefix = directoryPrefix ?? throw new ArgumentNullException(nameof(directoryPrefix));
        _filePrefix = filePrefix ?? throw new ArgumentNullException(nameof(filePrefix));
        _indentUnit = indentUnit ?? throw new ArgumentNullException(nameof(indentUnit));
    }

    public void RenderTree(IFileSystem fileSystem, IPath root, int depth, IOutput output)
    {
        if (fileSystem is null) throw new ArgumentNullException(nameof(fileSystem));
        if (root is null) throw new ArgumentNullException(nameof(root));
        if (output is null) throw new ArgumentNullException(nameof(output));

        if (depth < 1) depth = 1;

        string name = root.ToString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(name)) name = "/";

        RenderDirectory(fileSystem, root, name, depth, level: 0, output);
    }

    private void RenderDirectory(
        IFileSystem fileSystem,
        IPath directoryPath,
        string directoryName,
        int maxDepth,
        int level,
        IOutput output)
    {
        output.WriteLine(BuildIndent(level) + _directoryPrefix + directoryName);

        if (level + 1 >= maxDepth)
            return;

        IEnumerable<string> entries = fileSystem.List(directoryPath);

        foreach (string entryName in entries.Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            IPath child = directoryPath
                .CombineRelative(_pathParser.Parse(entryName))
                .Normalize();

            if (fileSystem.DirectoryExists(child))
            {
                RenderDirectory(fileSystem, child, entryName, maxDepth, level + 1, output);
            }
            else if (fileSystem.FileExists(child))
            {
                output.WriteLine(BuildIndent(level + 1) + _filePrefix + entryName);
            }
        }
    }

    private string BuildIndent(int level)
    {
        if (level <= 0) return string.Empty;
        return string.Concat(Enumerable.Repeat(_indentUnit, level));
    }
}
