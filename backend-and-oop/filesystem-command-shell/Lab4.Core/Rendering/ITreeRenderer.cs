using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystems;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Output;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Paths;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Rendering;

public interface ITreeRenderer
{
    void RenderTree(IFileSystem fileSystem, IPath root, int depth, IOutput output);
}
