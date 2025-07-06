using System.Numerics;

namespace Enigma.Machine;

/// <summary>Represents a pegboard connection between two <see cref="Letter"/> letters</summary>
//[DebuggerDisplay("PegboardWire = {IsUnplugged ? \"Unplugged\" : {EndA.Upper} + \" <-> \" + {EndB.Upper}}")]
public readonly struct PlugboardWire : IEqualityOperators<PlugboardWire, PlugboardWire, bool>, IEquatable<PlugboardWire>
{
    public static readonly PlugboardWire Unplugged = new PlugboardWire();

    public PlugboardWire() : this(Letter.Invalid, Letter.Invalid) { }
    public PlugboardWire(Letter endA, Letter endB)
    {
        if (endA == Letter.Invalid && endB == Letter.Invalid)
            return;

        if ((endA == Letter.Invalid && endB != Letter.Invalid) || (endA != Letter.Invalid && endB == Letter.Invalid))
            throw new InvalidOperationException("Invalid PegboardWire creation parameters");

        if (endA == endB)
            throw new InvalidOperationException("Cannot create a PegboardWire that plugs both ends into the same Alphabet letter");
        
        (EndA, EndB) = (endA, endB);
    }

    /// <summary>First letter this wire is plugged into</summary>
    public readonly Letter EndA = Letter.Invalid;

    /// <summary>Second letter this wire is plugged into</summary>
    public readonly Letter EndB = Letter.Invalid;

    /// <summary>Process the <see cref="Letter"/> transformation on <paramref name="input"/></summary>
    /// <returns><c>true</c> if the letter was transformed, otherwise <c>false</c></returns>
    public bool Process(ref Letter input)
    {
        // Ensure either both plugs are invalid, or neither plug is invalid
        // Debug.Assert(EndA != Alphabet.Invalid && EndB == Alphabet.Invalid);
        // Debug.Assert(EndA == Alphabet.Invalid && EndB != Alphabet.Invalid);

        if (EndA != Letter.Invalid && EndB == Letter.Invalid ||
            EndA == Letter.Invalid && EndB != Letter.Invalid)
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

    public          bool Equals(PlugboardWire other)                         => EndA.Equals(other.EndA)   && EndB.Equals(other.EndB);
    public override bool Equals(object?      obj)                           => obj is PlugboardWire other && Equals(other);
    public override int  GetHashCode()                                      => HashCode.Combine(EndA, EndB);
    public static   bool operator ==(PlugboardWire left, PlugboardWire right) => left.Equals(right);
    public static   bool operator !=(PlugboardWire left, PlugboardWire right) => !(left == right);

#if DEBUG
    public override string ToString()
    {
        return EndA == Letter.Invalid && EndB == Letter.Invalid ?
            "PegboardWire = Unplugged" :
            (EndA == Letter.Invalid && EndB != Letter.Invalid) || (EndA != Letter.Invalid && EndB == Letter.Invalid) ?
                "PegboardWire = Invalid" :
                $"PegboardWire = {EndA.Upper} <-> {EndB.Upper}";
    }
#endif
}