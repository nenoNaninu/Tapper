using System.Runtime.CompilerServices;
using System.Text;

namespace Tapper;

public readonly struct CodeWriter
{
    private readonly StringBuilder _stringBuilder;

    public CodeWriter() : this(new StringBuilder())
    {
    }

    public CodeWriter(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char value)
    {
        _stringBuilder.Append(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(string value)
    {
        this._stringBuilder.Append(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return _stringBuilder.ToString();
    }
}
