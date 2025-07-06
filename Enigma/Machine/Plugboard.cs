using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Enigma.Machine;

// public sealed class Enigma
// {
//     public sealed class Configuration { }
//
//
//     /// <summary>Create a new Enigma machine with the provided configuration</summary>
//     public static Enigma Create(Configuration configuration) { }
//
//     /// <summary>Create a new Enigma machine with a completely random configuration</summary>
//     /// <param name="generatedConfiguration">Enigma configuration generated. Can be used to actually decode
//     /// the messages encoded by the created Enigma machine</param>
//     public static Enigma CreateRandom(out Configuration generatedConfiguration) { }
// }

/// <summary>Connects <see cref="Alphabet"/> letters together via <see cref="PlugboardWire"/> to further enhance encryption</summary>
[DebuggerDisplay("Pegboard = {_wireCount} Wires")]
public struct Plugboard() //TODO rewrite everything here to use Unsafe+MemoryMarshal and get dangerous! (Because why not!)
{
    /// <summary>How many <see cref="PlugboardWire"/>s are in use. Up to 13 wires may be in use at a given time</summary>
    public byte WireCount => _wireCount;

    private byte _wireCount = 0;

    private readonly PlugboardWire[] _wires = new PlugboardWire[13];

    /// <summary>Plug in a new <see cref="PlugboardWire"/> to <paramref name="a"/> and <paramref name="b"/></summary>
    public void Plug(Alphabet a, Alphabet b)
    {
        if (_wireCount >= 13)
            throw new InvalidOperationException("Cannot plug more than 13 PegboardWires into the Pegboard");

        var range = _wireCount > 0 ? Range.EndAt(_wireCount - 1) : Range.All;
        foreach (var wire in _wires.AsSpan(range))
        {
            if (wire.EndA == a || wire.EndB == a || wire.EndA == b || wire.EndB == b)
                throw new InvalidOperationException("Cannot plug PegboardWire into an occupied slot");
        }

        _wires[_wireCount++] = new PlugboardWire(a, b);
    }

    /// <summary>Unplug both ends of the wire that is connected to <paramref name="a"/></summary>
    public void Unplug(Alphabet a)
    {
        if (a == Alphabet.Invalid)
            throw new InvalidOperationException("Cannot unplug PegboardWire from an invalid slot");

        if (_wireCount == 0)
            return;

        for (var i = 0; i < _wireCount; i++)
        {
            var wire = _wires[i];
            if (wire.EndA != a && wire.EndB != a)
                continue;

            (_wires[i], _wires[_wireCount - 1]) = (_wires[--_wireCount], PlugboardWire.Unplugged);
            return;
        }
    }

    /// <summary>Unplug all <see cref="PlugboardWire"/>, resetting the <see cref="Plugboard"/> to its original state</summary>
    public void UnplugAll()
    {
        Span<PlugboardWire> reset = stackalloc PlugboardWire[_wires.Length];
        reset.Clear();
        reset.CopyTo(_wires);
    }

    /// <summary>Provide <paramref name="input"/> to every live <see cref="PlugboardWire"/></summary>
    /// <returns><c>true</c> if the letter was transformed, otherwise <c>false</c></returns>
    public bool Process(ref Alphabet input)
    {
        foreach (var wire in _wires.AsSpan())
        {
            if (wire.Process(ref input))
                return true;
        }

        return false;
    }
}