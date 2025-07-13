using System.Numerics;

namespace Enigma.Machine.Plugboard;

/// <summary>Represents a Plugboard connection between two <see cref="Letter"/> letters</summary>
//[DebuggerDisplay("PlugboardWire = {IsUnplugged ? \"Unplugged\" : {EndA.Upper} + \" <-> \" + {EndB.Upper}}")]
public readonly struct PlugboardWire : IEqualityOperators<PlugboardWire, PlugboardWire, bool>, IEquatable<PlugboardWire>
{
    public static readonly PlugboardWire Unplugged = new PlugboardWire();

    public PlugboardWire() : this(Letter.Invalid, Letter.Invalid) { }
    public PlugboardWire(Letter endA, Letter endB)
    {
        if (endA == Letter.Invalid && endB == Letter.Invalid)
            return;

        if ((endA == Letter.Invalid && endB != Letter.Invalid) || (endA != Letter.Invalid && endB == Letter.Invalid))
            throw new InvalidOperationException("Invalid PlugboardWire creation parameters");

        if (endA == endB)
            throw new InvalidOperationException("Cannot create a PlugboardWire that plugs both ends into the same Alphabet letter");
        
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
            throw new InvalidOperationException("Cannot run PlugboardWire.Process(...) on an invalid PlugboardWire");

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
            "PlugboardWire = Unplugged" :
            (EndA == Letter.Invalid && EndB != Letter.Invalid) || (EndA != Letter.Invalid && EndB == Letter.Invalid) ?
                "PlugboardWire = Invalid" :
                $"PlugboardWire = {EndA.Upper} <-> {EndB.Upper}";
    }
#endif
}