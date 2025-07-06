using Enigma.Machine;

namespace Enigma;

/// <summary>Enigma machine. Encrypts/decrypts inputs via the individual components of the enigma machine</summary>
public struct Enigma(Plugboard plugs, RotorAssembly rotors)
{
    private readonly Keyboard      _keys   = default;
    private readonly Plugboard     _plugs  = plugs;
    private          RotorAssembly _rotors = rotors;
    private          Lightboard    _lights = default;

    /// <summary>Runs the Enigma algorithm for every character in the input sequence</summary>
    /// <returns>Enigma encrypted/decrypted input</returns>
    /// <exception cref="InvalidOperationException">If a non-alpha character is encountered</exception>
    public string Process(string input)
    {
        Span<char> output = stackalloc char[input.Length];
        input.CopyTo(output);
        Process(ref output);
        return output.ToString();
    }

    /// <summary>Runs the Enigma algorithm for every character in the input sequence in-place</summary>
    /// <exception cref="InvalidOperationException">If a non-alpha character is encountered</exception>
    public void Process(ref Span<char> data)
    {
        foreach (ref var c in data)
        {
            var l = _keys.Press(c);
            ProcessInternal(ref l);
            c = l.Upper;
        }
    }

    private void ProcessInternal(ref Letter letter)
    {
        // In this enigma implementation, the Keyboard is just the mechanism for converting CLR types to Letter. While the
        // real Enigma machine processes inputs by passing through they keyboard twice, this simulation does not do so
        //_keys.Process(ref letter);

        _plugs.Process(ref letter);
        _rotors.Advance();
        _rotors.Process(ref letter);
        _plugs.Process(ref letter);

        //_keys.Process(ref letter);

        _lights.Show(letter);
    }
}