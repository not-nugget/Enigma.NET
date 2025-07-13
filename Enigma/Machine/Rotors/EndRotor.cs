using System.Numerics;

namespace Enigma.Machine.Rotors;

/// <summary>Represents a Rotor that is stationary and will not roll over, effectively a <see cref="Letter"/> map</summary>
public readonly struct EndRotor
{
    /// <summary><see cref="EndRotor"/> that does not perform any <see cref="Letter"/> transformation</summary>
    public static readonly EndRotor Passthrough = new EndRotor([
        new ForwardLetter(Letter.A, Letter.A),
        new ForwardLetter(Letter.B, Letter.B),
        new ForwardLetter(Letter.C, Letter.C),
        new ForwardLetter(Letter.D, Letter.D),
        new ForwardLetter(Letter.E, Letter.E),
        new ForwardLetter(Letter.F, Letter.F),
        new ForwardLetter(Letter.G, Letter.G),
        new ForwardLetter(Letter.H, Letter.H),
        new ForwardLetter(Letter.I, Letter.I),
        new ForwardLetter(Letter.J, Letter.J),
        new ForwardLetter(Letter.K, Letter.K),
        new ForwardLetter(Letter.L, Letter.L),
        new ForwardLetter(Letter.M, Letter.M),
        new ForwardLetter(Letter.N, Letter.N),
        new ForwardLetter(Letter.O, Letter.O),
        new ForwardLetter(Letter.P, Letter.P),
        new ForwardLetter(Letter.Q, Letter.Q),
        new ForwardLetter(Letter.R, Letter.R),
        new ForwardLetter(Letter.S, Letter.S),
        new ForwardLetter(Letter.T, Letter.T),
        new ForwardLetter(Letter.U, Letter.U),
        new ForwardLetter(Letter.V, Letter.V),
        new ForwardLetter(Letter.W, Letter.W),
        new ForwardLetter(Letter.X, Letter.X),
        new ForwardLetter(Letter.Y, Letter.Y),
        new ForwardLetter(Letter.Z, Letter.Z),
    ]);

    /// <summary><see cref="EndRotor"/> that transforms incoming <see cref="Letter"/>s to their alphabetical inverse</summary>
    public static readonly EndRotor Inverse = new EndRotor([
        new ForwardLetter(Letter.A, Letter.Z),
        new ForwardLetter(Letter.B, Letter.Y),
        new ForwardLetter(Letter.C, Letter.X),
        new ForwardLetter(Letter.D, Letter.W),
        new ForwardLetter(Letter.E, Letter.V),
        new ForwardLetter(Letter.F, Letter.U),
        new ForwardLetter(Letter.G, Letter.T),
        new ForwardLetter(Letter.H, Letter.S),
        new ForwardLetter(Letter.I, Letter.R),
        new ForwardLetter(Letter.J, Letter.Q),
        new ForwardLetter(Letter.K, Letter.P),
        new ForwardLetter(Letter.L, Letter.O),
        new ForwardLetter(Letter.M, Letter.N),
        new ForwardLetter(Letter.N, Letter.M),
        new ForwardLetter(Letter.O, Letter.L),
        new ForwardLetter(Letter.P, Letter.K),
        new ForwardLetter(Letter.Q, Letter.J),
        new ForwardLetter(Letter.R, Letter.I),
        new ForwardLetter(Letter.S, Letter.H),
        new ForwardLetter(Letter.T, Letter.G),
        new ForwardLetter(Letter.U, Letter.F),
        new ForwardLetter(Letter.V, Letter.E),
        new ForwardLetter(Letter.W, Letter.D),
        new ForwardLetter(Letter.X, Letter.C),
        new ForwardLetter(Letter.Y, Letter.B),
        new ForwardLetter(Letter.Z, Letter.A),
    ]);

    private readonly ForwardLetter[] _pairs = new ForwardLetter[Letter.AlphabetSize];

    public EndRotor(ReadOnlySpan<ForwardLetter> pairs)
    {
        if (pairs.Length != Letter.AlphabetSize)
            throw new ArgumentOutOfRangeException(nameof(pairs), null, "ShiftMap's ForwardLetter array must contain exactly 26 elements");

        ulong bitAccumulator = 0;
        for (var i = 0; i < Letter.AlphabetSize; i++)
        {
            var s = pairs[i];
            if (s.In == Letter.Invalid || s.Out == Letter.Invalid)
                throw new ArgumentException("ShiftMap may not map to or from an invalid Alphabet letter");
            if (BitOperations.PopCount(bitAccumulator) != 2 * i)
                throw new ArgumentException("Alphabet may only occur once per tuple item within the ShiftMap array");

            bitAccumulator |= (ulong)s.In.Value << 32;
            bitAccumulator |= s.Out.Value;
            _pairs[i]      =  pairs[i];
        }
    }

    /// <summary>Process the incoming letter by redirecting it to the</summary>
    /// <param name="letter"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Process(ref Letter letter)
    {
        foreach (var pair in _pairs.AsSpan())
        {
            if (pair.In != letter)
                continue;

            letter = pair.Out;
            return;
        }

        throw new InvalidOperationException("Reflector encountered an invalid Letter when processing");
    }

    /// <summary>Creates an <see cref="EndRotor"/> from a 26 character input string, mapping the sequential
    /// alphabet to and from the input character's index at that position</summary>
    public static EndRotor Parse(ReadOnlySpan<char> input)
    {
        if (input.Length is not Letter.AlphabetSize)
            throw new InvalidOperationException("Invalid Rotor string format encountered");

        Span<ForwardLetter> letters = stackalloc ForwardLetter[input.Length];
        for (var i = 0; i < Letter.AlphabetSize; i++)
        {
            var a = Letter.Cache[i];
            var b = Letter.FromChar(input[i]);
            letters[i] = new ForwardLetter(a, b);
        }

        return new EndRotor(letters);
    }
}