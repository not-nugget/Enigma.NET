using Enigma.Machine;

using Shouldly;

namespace Enigma.Tests;

public class PlugboardWireTests
{
    [Fact]
    public void DefaultPlugboardWire_ShouldEqual_UnpluggedPlugboardWire()
    {
        var a = new PlugboardWire();
        var e = PlugboardWire.Unplugged;

        a.ShouldBe(e);
    }

    [Fact]
    public void UnpluggedPlugboardWire_DoesNotModifyAlphabet_WhenProcessCalled()
    {
        var w = PlugboardWire.Unplugged;
        var a = Alphabet.A;
        var e = Alphabet.A;

        w.Process(ref a);

        a.ShouldBe(e);
    }

    [Fact]
    public void InvalidPlugboardWire_Throws_OnConstruct() { Should.Throw<InvalidOperationException>(() => new PlugboardWire(Alphabet.Invalid, Alphabet.A)); }

    [Fact]
    public void PlugboardWire_Throws_WhenConnectingToAndFromTheSameLetter()
    {
        var w = new PlugboardWire(Alphabet.Invalid, Alphabet.A);
        var a = Alphabet.A;

        Should.Throw<InvalidOperationException>(() => w.Process(ref a));
    }

    [Fact]
    public void ValidPlugboardWire_TransformsAllInputsToAllOutputs_Successfully()
    {
        var permutations = Alphabet
            .Cache
            .SelectMany(a => Alphabet.Cache.Where(b => a != b).Select(b => new PlugboardWire(a, b)));

        foreach (var wire in permutations)
        {
            foreach (var letter in Alphabet.Cache)
            {
                var actual   = letter;
                var expected = wire.EndA == letter ? wire.EndB : letter;
                
                wire.Process(ref actual);
                actual.ShouldBe(expected);
            }
        }
    }
}