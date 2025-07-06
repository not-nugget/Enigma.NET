using System.Diagnostics;
using System.Numerics;

namespace Enigma;

/// <summary>Available Enigma alphabet</summary>
[DebuggerDisplay("Alphabet = {Upper}")]
public readonly struct Letter : IEqualityOperators<Letter, Letter, bool>, IEquatable<Letter>
{
    internal const int AlphabetSize = 26;
    
    /// <summary>Invalid <see cref="Letter"/> that will cause errors when used</summary>
    public static readonly Letter Invalid = new Letter();

    public static readonly Letter A = new Letter(0b01100001_01000001_00000000000000000000000001);
    public static readonly Letter B = new Letter(0b01100010_01000010_00000000000000000000000010);
    public static readonly Letter C = new Letter(0b01100011_01000011_00000000000000000000000100);
    public static readonly Letter D = new Letter(0b01100100_01000100_00000000000000000000001000);
    public static readonly Letter E = new Letter(0b01100101_01000101_00000000000000000000010000);
    public static readonly Letter F = new Letter(0b01100110_01000110_00000000000000000000100000);
    public static readonly Letter G = new Letter(0b01100111_01000111_00000000000000000001000000);
    public static readonly Letter H = new Letter(0b01101000_01001000_00000000000000000010000000);
    public static readonly Letter I = new Letter(0b01101001_01001001_00000000000000000100000000);
    public static readonly Letter J = new Letter(0b01101010_01001010_00000000000000001000000000);
    public static readonly Letter K = new Letter(0b01101011_01001011_00000000000000010000000000);
    public static readonly Letter L = new Letter(0b01101100_01001100_00000000000000100000000000);
    public static readonly Letter M = new Letter(0b01101101_01001101_00000000000001000000000000);
    public static readonly Letter N = new Letter(0b01101110_01001110_00000000000010000000000000);
    public static readonly Letter O = new Letter(0b01101111_01001111_00000000000100000000000000);
    public static readonly Letter P = new Letter(0b01110000_01010000_00000000001000000000000000);
    public static readonly Letter Q = new Letter(0b01110001_01010001_00000000010000000000000000);
    public static readonly Letter R = new Letter(0b01110010_01010010_00000000100000000000000000);
    public static readonly Letter S = new Letter(0b01110011_01010011_00000001000000000000000000);
    public static readonly Letter T = new Letter(0b01110100_01010100_00000010000000000000000000);
    public static readonly Letter U = new Letter(0b01110101_01010101_00000100000000000000000000);
    public static readonly Letter V = new Letter(0b01110110_01010110_00001000000000000000000000);
    public static readonly Letter W = new Letter(0b01110111_01010111_00010000000000000000000000);
    public static readonly Letter X = new Letter(0b01111000_01011000_00100000000000000000000000);
    public static readonly Letter Y = new Letter(0b01111001_01011001_01000000000000000000000000);
    public static readonly Letter Z = new Letter(0b01111010_01011010_10000000000000000000000000);

    internal static readonly Letter[] Cache = [A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z];

    private const ulong LowerMask = 0b11111111_00000000_00000000000000000000000000;
    private const ulong UpperMask = 0b00000000_11111111_00000000000000000000000000;
    private const ulong ValueMask = 0b00000000_00000000_11111111111111111111111111;

    public readonly char Upper;
    public readonly char Lower;
    public readonly uint Value;

    public Letter() : this(0UL) { }
    private Letter(ulong v)
    {
        Upper = (char)(byte)((v & UpperMask) >> 26);
        Lower = (char)(byte)((v & LowerMask) >> (26 + 8));
        Value = (uint)(v & ValueMask);
    }

    public          bool Equals(Letter other) => Value == other.Value;
    public override bool Equals(object?  obj)   => obj is Letter other && Equals(other);
    public override int  GetHashCode()          => Value.GetHashCode();

    /// <summary>Get the <see cref="Letter"/> that represents <paramref name="c"/></summary>
    public static Letter FromChar(char c)
    {
        foreach (var a in Cache.AsSpan())
        {
            if (a.Lower == c || a.Upper == c)
                return a;
        }

        return Invalid;
    }
    /// <summary>Get the <see cref="Letter"/> at the provided index, between 0 and 25 (A-Z)</summary>
    public static Letter FromIndex(byte i) => i > 25 ? Invalid : Cache[i];

    public static bool operator ==(Letter left, Letter right) => left.Equals(right);
    public static bool operator !=(Letter left, Letter right) => !(left == right);
}