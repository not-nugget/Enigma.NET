using System.Numerics;

namespace Enigma.Machine;

/// <summary>Rotor mechanism that cyclically transforms <see cref="Letter"/> letters</summary>
public struct Rotor
{
    public static Rotor Default = new Rotor(Letter.Cache);
    
    private const byte Rollover = 26;

    public byte Current => (byte)(_current + 1);

    private readonly Letter[] _shiftMap = new Letter[Rollover];
    private          byte       _current;

    public Rotor(Letter[] shiftMap, byte current = 0)
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

        _current = current;
    }

    /// <summary>Process the incoming letter</summary>
    public void Process(ref Letter i) 
        => i = _shiftMap[_current];

    /// <summary>Advance the rotor and return a rollover indication</summary>
    /// <returns><c>true</c> if the rotor completed a full revolution and has
    /// reset to zero, otherwise <c>false</c></returns>
    public bool Advance()
    {
        if (++_current != Rollover)
            return false;

        _current = 0;
        return true;
    }
}