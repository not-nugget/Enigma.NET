using System.Numerics;

namespace Enigma.Machine;

/// <summary>Represents a Rotor that is stationary and will not roll over, effectively a <see cref="Letter"/> map</summary>
public readonly struct EndRotor
{
    /// <summary><see cref="EndRotor"/> that does not perform any <see cref="Letter"/> transformation</summary>
    public static readonly EndRotor Passthrough = new EndRotor([
        (Letter.A, Letter.A),
        (Letter.B, Letter.B),
        (Letter.C, Letter.C),
        (Letter.D, Letter.D),
        (Letter.E, Letter.E),
        (Letter.F, Letter.F),
        (Letter.G, Letter.G),
        (Letter.H, Letter.H),
        (Letter.I, Letter.I),
        (Letter.J, Letter.J),
        (Letter.K, Letter.K),
        (Letter.L, Letter.L),
        (Letter.M, Letter.M),
        (Letter.N, Letter.N),
        (Letter.O, Letter.O),
        (Letter.P, Letter.P),
        (Letter.Q, Letter.Q),
        (Letter.R, Letter.R),
        (Letter.S, Letter.S),
        (Letter.T, Letter.T),
        (Letter.U, Letter.U),
        (Letter.V, Letter.V),
        (Letter.W, Letter.W),
        (Letter.X, Letter.X),
        (Letter.Y, Letter.Y),
        (Letter.Z, Letter.Z),
    ]);

    /// <summary><see cref="EndRotor"/> that transforms incoming <see cref="Letter"/>s to their alphabetical inverse</summary>
    public static readonly EndRotor Inverse = new EndRotor([
        (Letter.A, Letter.Z),
        (Letter.B, Letter.Y),
        (Letter.C, Letter.X),
        (Letter.D, Letter.W),
        (Letter.E, Letter.V),
        (Letter.F, Letter.U),
        (Letter.G, Letter.T),
        (Letter.H, Letter.S),
        (Letter.I, Letter.R),
        (Letter.J, Letter.Q),
        (Letter.K, Letter.P),
        (Letter.L, Letter.O),
        (Letter.M, Letter.N),
        (Letter.N, Letter.M),
        (Letter.O, Letter.L),
        (Letter.P, Letter.K),
        (Letter.Q, Letter.J),
        (Letter.R, Letter.I),
        (Letter.S, Letter.H),
        (Letter.T, Letter.G),
        (Letter.U, Letter.F),
        (Letter.V, Letter.E),
        (Letter.W, Letter.D),
        (Letter.X, Letter.C),
        (Letter.Y, Letter.B),
        (Letter.Z, Letter.A),
    ]);

    private readonly (Letter In, Letter Out)[] _pairs = new (Letter In, Letter Out)[Letter.AlphabetSize];

    public EndRotor((Letter In, Letter Out)[] pairs)
    {
        if (pairs.Length != Letter.AlphabetSize)
            throw new ArgumentOutOfRangeException(nameof(pairs), pairs, "ShiftMap array must contain exactly 26 elements");

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
}