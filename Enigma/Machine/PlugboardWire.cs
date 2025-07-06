using System.Numerics;

namespace Enigma.Machine;

/// <summary>Represents a pegboard connection between two <see cref="Alphabet"/> letters</summary>
//[DebuggerDisplay("PegboardWire = {IsUnplugged ? \"Unplugged\" : {EndA.Upper} + \" <-> \" + {EndB.Upper}}")]
public readonly struct PegboardWire : IEqualityOperators<PegboardWire, PegboardWire, bool>, IEquatable<PegboardWire>
{
    public static readonly PegboardWire Unplugged = new PegboardWire();

    public PegboardWire() : this(Alphabet.Invalid, Alphabet.Invalid) { }
    public PegboardWire(Alphabet endA, Alphabet endB)
    {
        if (endA == Alphabet.Invalid && endB == Alphabet.Invalid)
            return;

        if ((endA == Alphabet.Invalid && endB != Alphabet.Invalid) || (endA != Alphabet.Invalid && endB == Alphabet.Invalid))
            throw new InvalidOperationException("Invalid PegboardWire creation parameters");

        if (endA == endB)
            throw new InvalidOperationException("Cannot create a PegboardWire that plugs both ends into the same Alphabet letter");
        
        (EndA, EndB) = (endA, endB);
    }

    /// <summary>First letter this wire is plugged into</summary>
    public readonly Alphabet EndA = Alphabet.Invalid;

    /// <summary>Second letter this wire is plugged into</summary>
    public readonly Alphabet EndB = Alphabet.Invalid;

    /// <summary>Process the <see cref="Alphabet"/> transformation on <paramref name="input"/></summary>
    /// <returns><c>true</c> if the letter was transformed, otherwise <c>false</c></returns>
    public bool Process(ref Alphabet input)
    {
        // Ensure either both plugs are invalid, or neither plug is invalid
        // Debug.Assert(EndA != Alphabet.Invalid && EndB == Alphabet.Invalid);
        // Debug.Assert(EndA == Alphabet.Invalid && EndB != Alphabet.Invalid);

        if (EndA != Alphabet.Invalid && EndB == Alphabet.Invalid ||
            EndA == Alphabet.Invalid && EndB != Alphabet.Invalid)
            throw new InvalidOperationException("Cannot run PegboardWire.Process(...) on an invalid PegboardWire");

        if (EndA == input)
        {
            input = EndB;
            return true;
        }

        if (EndB == input)
        {
            input = EndA;
            return true;
        }

        return false;
    }

    public          bool Equals(PegboardWire other)                         => EndA.Equals(other.EndA)   && EndB.Equals(other.EndB);
    public override bool Equals(object?      obj)                           => obj is PegboardWire other && Equals(other);
    public override int  GetHashCode()                                      => HashCode.Combine(EndA, EndB);
    public static   bool operator ==(PegboardWire left, PegboardWire right) => left.Equals(right);
    public static   bool operator !=(PegboardWire left, PegboardWire right) => !(left == right);

#if DEBUG
    public override string ToString()
    {
        return EndA == Alphabet.Invalid && EndB == Alphabet.Invalid ?
            "PegboardWire = Unplugged" :
            (EndA == Alphabet.Invalid && EndB != Alphabet.Invalid) || (EndA != Alphabet.Invalid && EndB == Alphabet.Invalid) ?
                "PegboardWire = Invalid" :
                $"PegboardWire = {EndA.Upper} <-> {EndB.Upper}";
    }
#endif
}