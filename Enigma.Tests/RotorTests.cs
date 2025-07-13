using Enigma.Machine.Rotors;

using Shouldly;

namespace Enigma.Tests;

public class RotorTests
{
    [Fact]
    public void Rotor_ParseTooShortString_ThrowsInvalidOperationException()
    {
        const string P = "ABCDEFGHIJKLMNOPQRSTUVWXYZG";
        Should.Throw<InvalidOperationException>(() => Rotor.Parse(P));
    }
    
    [Fact]
    public void Rotor_ParseTooLongString_ThrowsInvalidOperationException()
    {
        const string P = "ABCDEFGHIJKLMNOPQRSTUVWXYZG13ABD";
        Should.Throw<InvalidOperationException>(() => Rotor.Parse(P));
    }
    
    [Fact]
    public void Rotor_ParseValidString_ReturnsCorrectRotor()
    {
        const string P = "ABCDEFGHIJKLMNOPQRSTUVWXYZG13";

        var actual    = Rotor.Parse(P);
        var exptected = new Rotor(Letter.Cache, Letter.G, 13);
        
        actual.ShouldBe(exptected);
    }
    
}