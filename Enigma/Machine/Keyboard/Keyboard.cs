namespace Enigma.Machine.Keyboard;

/// <summary>Enters the Enigma system by converting various inputs into their respective <see cref="Letter"/> outputs</summary>
public readonly struct Keyboard()
{
    private readonly Letter[] _keys = Letter.Cache;

    /// <summary>Converts <paramref name="key"/> into its <see cref="Letter"/> counterpart, or returns <see cref="Letter.Invalid"/></summary>
    public Letter Press(char key)
    {
        foreach (var letter in _keys.AsSpan())
        {
            if (key == letter.Upper || key == letter.Lower)
                return letter;
        }

        return Letter.Invalid;
    }
}