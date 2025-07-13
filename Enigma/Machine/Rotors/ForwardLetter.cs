using System.Diagnostics;

namespace Enigma.Machine.Rotors;

/// <summary>Rewrites an incoming letter to an outgoing letter only when applicable. Supports two-way forwarding</summary>
[DebuggerDisplay("ForwardLetter = {In} {TwoWay ? '<' : ''}-> {Out}")]
public readonly struct ForwardLetter(Letter @in, Letter @out, bool twoWay = false)
{
    public readonly Letter In  = @in;
    public readonly Letter Out = @out;

    public readonly bool TwoWay = twoWay;

    public ForwardLetter() : this(Letter.Invalid, Letter.Invalid) { }

    /// <summary>Rewrites the incoming <paramref name="letter"/> if it is equal to <see cref="In"/></summary>
    public bool Process(ref Letter letter)
    {
        if (letter == In)
        {
            letter = Out;
            return true;
        }

        if (!TwoWay || letter != Out)
            return false;

        letter = In;
        return true;
    }
}