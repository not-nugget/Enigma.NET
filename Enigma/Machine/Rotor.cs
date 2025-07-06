using System.Numerics;

namespace Enigma.Machine;

/// <summary>Rotor mechanism that cyclically transforms <see cref="Letter"/> letters</summary>
public struct Rotor
{
    private const byte Rollover = 26;

    public byte Current => (byte)(_current + 1);

    private readonly Letter[] _shiftMap = new Letter[Rollover];
    private readonly Letter   _notch;
    private          byte     _current;

    public Rotor(Letter[] shiftMap, Letter notch, byte current = 0)
    {
        if (shiftMap.Length != Rollover)
            throw new ArgumentOutOfRangeException(nameof(shiftMap), shiftMap, "ShiftMap array must contain exactly 26 elements");

        uint bitAccumulator = 0;
        for (var i = 0; i < Rollover; i++)
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
        if (++_current == Rollover)
            _current = 0;

        return forwardAdvance;
    }
}