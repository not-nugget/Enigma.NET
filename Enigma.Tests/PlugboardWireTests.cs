using Enigma.Machine;

using Shouldly;

namespace Enigma.Tests;

public class PegboardWireTests
{
    [Fact]
    public void DefaultPegboardWire_ShouldEqual_UnpluggedPegboardWire()
    {
        var a = new PegboardWire();
        var e = PegboardWire.Unplugged;

        a.ShouldBe(e);
    }

    [Fact]
    public void UnpluggedPegboardWire_DoesNotModifyAlphabet_WhenProcessCalled()
    {
        var w = PegboardWire.Unplugged;
        var a = Alphabet.A;
        var e = Alphabet.A;

        w.Process(ref a);

        a.ShouldBe(e);
    }

    [Fact]
    public void InvalidPegboardWire_Throws_OnConstruct() { Should.Throw<InvalidOperationException>(() => new PegboardWire(Alphabet.Invalid, Alphabet.A)); }

    [Fact]
    public void PegboardWire_Throws_WhenConnectingToAndFromTheSameLetter()
    {
        var w = new PegboardWire(Alphabet.Invalid, Alphabet.A);
        var a = Alphabet.A;

        Should.Throw<InvalidOperationException>(() => w.Process(ref a));
    }

    [Fact]
    public void ValidPegboardWire_TransformsAllInputsToAllOutputs_Successfully()
    {
        var permutations = Alphabet
            .Cache
            .SelectMany(a => Alphabet.Cache.Where(b => a != b).Select(b => new PegboardWire(a, b)));

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