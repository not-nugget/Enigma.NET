namespace Enigma.Machine.Rotors;

/// <summary>Represents the three <see cref="Rotor"/>s, and the <see cref="EndRotor"/></summary>
public struct RotorAssembly(EndRotor entryRotor, Rotor a, Rotor b, Rotor c, EndRotor reflector)
{
    private readonly EndRotor _e = entryRotor;
    private          Rotor    _a = a;
    private          Rotor    _b = b;
    private          Rotor    _c = c;
    private readonly EndRotor _r = reflector;

    public RotorAssembly() : this(EndRotor.Passthrough, default, default, default, EndRotor.Inverse) { }

    /// <summary>Process a <see cref="Letter"/> by passing it through each rotor in the assembly</summary>
    public readonly void Process(ref Letter letter)
    {
        _e.Process(ref letter);
        
        _a.Process(ref letter);
        _b.Process(ref letter);
        _c.Process(ref letter);

        _r.Process(ref letter);

        _c.Process(ref letter);
        _b.Process(ref letter);
        _a.Process(ref letter);
        
        _e.Process(ref letter);
    }

    /// <summary>Advance each internal rotor, honoring notch and rollover positions</summary>
    public void Advance() 
        => _ = _a.Advance() && _b.Advance() && _c.Advance();
}