using System.Numerics;

namespace Enigma.Machine.Rotors;

/// <summary>Rotor mechanism that cyclically transforms <see cref="Letter"/> letters</summary>
public struct Rotor
{
    private const byte AlphabetSize = 26;

    public byte Current => (byte)(_current + 1);

    private readonly Letter[] _shiftMap = new Letter[AlphabetSize];
    private readonly Letter   _notch;
    private          byte     _current;

    public Rotor(ReadOnlySpan<Letter> shiftMap, Letter notch, byte current = 0)
    {
        if (shiftMap.Length != AlphabetSize)
            throw new ArgumentOutOfRangeException(nameof(shiftMap), null, "ShiftMap array must contain exactly 26 elements");

        uint bitAccumulator = 0;
        for (var i = 0; i < AlphabetSize; i++)
        {
            var s = shiftMap[i];
            if (s == Letter.Invalid)
                throw new ArgumentException("ShiftMap may not map to or from an invalid Alphabet letter");
            if (BitOperations.PopCount(bitAccumulator) != 2 * i)
                throw new ArgumentException("Alphabet may only occur once per tuple item within the ShiftMap array");

            bitAccumulator |= shiftMap[i].Value;
            _shiftMap[i]   =  shiftMap[i];
        }

        _notch   = notch;
        _current = current;
    }
    public Rotor() : this(Letter.Cache, Letter.A, 0) { }

    /// <summary>Process the incoming letter</summary>
    public readonly void Process(ref Letter i)
        => i = _shiftMap[_current];

    /// <summary>Advance the rotor and return a forward advancement indication</summary>
    /// <returns><c>true</c> if the rotor advanced beyond is notch <see cref="Letter"/>, meaning
    /// that the subsequent <see cref="Rotor"/> should also be advanced, otherwise <c>false</c></returns>
    public bool Advance()
    {
        var forwardAdvance = _shiftMap[_current] == _notch;
        if (++_current == AlphabetSize)
            _current = 0;

        return forwardAdvance;
    }

    /// <summary>Creates a <see cref="Rotor"/> from a 26 character input string, followed by a single character
    /// to indicate the letter notch, followed by a number between 1 and 25 to indicate the current position of
    /// the rotor, formatted like so: "ABCD...A15" where each letter is the transformed output based on the
    /// original alphabetic index</summary>
    public static Rotor Parse(ReadOnlySpan<char> input)
    {
        if (input.Length is not (28 or 29))
            throw new InvalidOperationException("Invalid Rotor string format encountered");

        Span<Letter> output = stackalloc Letter[AlphabetSize];
        for (var i = 0; i < 26; i++)
        {
            var c = input[i];
            if (c is not ((>= 'A' and <= 'Z') or (>= 'a' and <= 'z')))
                throw new InvalidOperationException("Invalid Rotor character encountered");

            output[i] = Letter.FromChar(c);
        }

        var notch   = Letter.FromChar(input[26]);
        var current = byte.Parse(input[27..]);

        return new Rotor(output, notch, current);
    }
}