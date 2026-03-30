namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing;

public interface ITokenIterator
{
    bool MoveNext();
    string Current { get; }
    int Position { get; }
    void SetPosition(int position);
}
