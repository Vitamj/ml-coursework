using System.Text;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsing;

public sealed class LineTokenIterator : ITokenIterator
{
    private readonly string[] _tokens;

    public LineTokenIterator(string line)
    {
        _tokens = Tokenize(line);
        Position = -1;
    }

    public int Position { get; private set; }

    public string Current
        => Position >= 0 && Position < _tokens.Length
            ? _tokens[Position]
            : throw new InvalidOperationException("No current token.");

    public bool MoveNext()
    {
        if (Position + 1 >= _tokens.Length) return false;
        Position++;
        return true;
    }

    public void SetPosition(int position) => Position = position;

    private static string[] Tokenize(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return Array.Empty<string>();

        var tokens = new List<string>();
        var sb = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line.Trim())
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (char.IsWhiteSpace(c) && !inQuotes)
            {
                Flush();
            }
            else
            {
                sb.Append(c);
            }
        }

        Flush();
        return tokens.ToArray();

        void Flush()
        {
            if (sb.Length == 0) return;
            tokens.Add(sb.ToString());
            sb.Clear();
        }
    }
}
