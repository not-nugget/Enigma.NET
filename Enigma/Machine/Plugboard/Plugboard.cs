using System.Diagnostics;

namespace Enigma.Machine.Plugboard;

/// <summary>Connects <see cref="Letter"/> letters together via <see cref="PlugboardWire"/> to further enhance encryption</summary>
[DebuggerDisplay("Plugboard = {_wireCount} Wires")]
public struct Plugboard() //TODO rewrite everything here to use Unsafe+MemoryMarshal and get dangerous! (Because why not!)
{
    public const int PlugboardWires = 10;

    /// <summary>How many <see cref="PlugboardWire"/>s are in use. Up to 13 wires may be in use at a given time</summary>
    public byte WireCount => _wireCount;

    private byte _wireCount = 0;

    private readonly PlugboardWire[] _wires = new PlugboardWire[PlugboardWires]; //TODO the video I am referencing says that the machine would only have *up to* 10 wires connected at a time, do I wnat to be 100% accurate, or do i want to take some liberties?

    /// <summary>Plug in a new <see cref="PlugboardWire"/> to <paramref name="a"/> and <paramref name="b"/></summary>
    public void Plug(Letter a, Letter b)
    {
        if (_wireCount >= PlugboardWires)
            throw new InvalidOperationException("Cannot plug more than 10 PlugboardWires into the Plugboard");

        var range = _wireCount > 0 ? Range.EndAt(_wireCount - 1) : Range.All;
        foreach (var wire in _wires.AsSpan(range))
        {
            if (wire.EndA == a || wire.EndB == a || wire.EndA == b || wire.EndB == b)
                throw new InvalidOperationException("Cannot plug PlugboardWire into an occupied slot");
        }

        _wires[_wireCount++] = new PlugboardWire(a, b);
    }

    /// <summary>Unplug both ends of the wire that is connected to <paramref name="a"/></summary>
    public void Unplug(Letter a)
    {
        if (a == Letter.Invalid)
            throw new InvalidOperationException("Cannot unplug PlugboardWire from an invalid slot");

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
    public readonly bool Process(ref Letter input)
    {
        foreach (var wire in _wires.AsSpan())
        {
            if (wire.Process(ref input))
                return true;
        }

        return false;
    }

    /// <summary>Creates a <see cref="Plugboard"/> from an input string formatted like so: "ABCD..."
    /// where each pair of letters indicates each end of a plugboard connection</summary>
    /// <exception cref="InvalidOperationException">Thrown when the incoming string is not in the correct
    /// format, or if an attempt to plug into the same slot twice</exception>
    public static Plugboard Parse(ReadOnlySpan<char> input)
    {
        if (input is { Length: > 20 } || input.Length % 2 != 0)
            throw new InvalidOperationException("Invalid Plugboard string format encountered");
        
        var                 plugboard = new Plugboard();
        for (var i = 0; i < input.Length; i += 2)
        {
            var (a, b) = (input[i], input[i + 1]);
            if (a is not (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') || b is not (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
                throw new InvalidOperationException("Invalid Plugboard character encountered");

            plugboard.Plug(Letter.FromChar(a), Letter.FromChar(b));
        }

        return plugboard;
    }
}