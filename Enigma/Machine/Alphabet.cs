using System.Diagnostics;
using System.Numerics;

namespace Enigma.Machine;

/// <summary>Available Enigma alphabet</summary>
[DebuggerDisplay("Alphabet = {Upper}")]
public readonly struct Alphabet : IEqualityOperators<Alphabet, Alphabet, bool>, IEquatable<Alphabet>
{
    /// <summary>Invalid <see cref="Alphabet"/> that will cause errors when used</summary>
    public static readonly Alphabet Invalid = new Alphabet();

    public static readonly Alphabet A = new Alphabet(0b01100001_01000001_00000000000000000000000001);
    public static readonly Alphabet B = new Alphabet(0b01100010_01000010_00000000000000000000000010);
    public static readonly Alphabet C = new Alphabet(0b01100011_01000011_00000000000000000000000100);
    public static readonly Alphabet D = new Alphabet(0b01100100_01000100_00000000000000000000001000);
    public static readonly Alphabet E = new Alphabet(0b01100101_01000101_00000000000000000000010000);
    public static readonly Alphabet F = new Alphabet(0b01100110_01000110_00000000000000000000100000);
    public static readonly Alphabet G = new Alphabet(0b01100111_01000111_00000000000000000001000000);
    public static readonly Alphabet H = new Alphabet(0b01101000_01001000_00000000000000000010000000);
    public static readonly Alphabet I = new Alphabet(0b01101001_01001001_00000000000000000100000000);
    public static readonly Alphabet J = new Alphabet(0b01101010_01001010_00000000000000001000000000);
    public static readonly Alphabet K = new Alphabet(0b01101011_01001011_00000000000000010000000000);
    public static readonly Alphabet L = new Alphabet(0b01101100_01001100_00000000000000100000000000);
    public static readonly Alphabet M = new Alphabet(0b01101101_01001101_00000000000001000000000000);
    public static readonly Alphabet N = new Alphabet(0b01101110_01001110_00000000000010000000000000);
    public static readonly Alphabet O = new Alphabet(0b01101111_01001111_00000000000100000000000000);
    public static readonly Alphabet P = new Alphabet(0b01110000_01010000_00000000001000000000000000);
    public static readonly Alphabet Q = new Alphabet(0b01110001_01010001_00000000010000000000000000);
    public static readonly Alphabet R = new Alphabet(0b01110010_01010010_00000000100000000000000000);
    public static readonly Alphabet S = new Alphabet(0b01110011_01010011_00000001000000000000000000);
    public static readonly Alphabet T = new Alphabet(0b01110100_01010100_00000010000000000000000000);
    public static readonly Alphabet U = new Alphabet(0b01110101_01010101_00000100000000000000000000);
    public static readonly Alphabet V = new Alphabet(0b01110110_01010110_00001000000000000000000000);
    public static readonly Alphabet W = new Alphabet(0b01110111_01010111_00010000000000000000000000);
    public static readonly Alphabet X = new Alphabet(0b01111000_01011000_00100000000000000000000000);
    public static readonly Alphabet Y = new Alphabet(0b01111001_01011001_01000000000000000000000000);
    public static readonly Alphabet Z = new Alphabet(0b01111010_01011010_10000000000000000000000000);

    internal static readonly Alphabet[] Cache = [A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z];

    private const ulong LowerMask = 0b11111111_00000000_00000000000000000000000000;
    private const ulong UpperMask = 0b00000000_11111111_00000000000000000000000000;
    private const ulong ValueMask = 0b00000000_00000000_11111111111111111111111111;

    public readonly char  Upper;
    public readonly char  Lower;
    public readonly ulong Value;

    public Alphabet() : this(0UL) { }
    private Alphabet(ulong v)
    {
        Upper = (char)(byte)((v & UpperMask) >> 26);
        Lower = (char)(byte)((v & LowerMask) >> (26 + 8));
        Value = v & ValueMask;
    }

    public          bool Equals(Alphabet other) => Value == other.Value;
    public override bool Equals(object?  obj)   => obj is Alphabet other && Equals(other);
    public override int  GetHashCode()          => Value.GetHashCode();

    /// <summary>Get the <see cref="Alphabet"/> that represents <paramref name="c"/></summary>
    public static Alphabet FromChar(char c)
    {
        foreach (var a in Cache.AsSpan())
        {
            if (a.Lower == c || a.Upper == c)
                return a;
        }

        return Invalid;
    }

    public static bool operator ==(Alphabet left, Alphabet right) => left.Equals(right);
    public static bool operator !=(Alphabet left, Alphabet right) => !(left == right);
}