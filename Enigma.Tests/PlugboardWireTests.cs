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
        var a = Letter.A;
        var e = Letter.A;

        w.Process(ref a);

        a.ShouldBe(e);
    }

    [Fact]
    public void InvalidPlugboardWire_Throws_WhenConstruct()
        => Should.Throw<InvalidOperationException>(() => new PlugboardWire(Letter.Invalid, Letter.A));

    [Fact]
    public void SameLetterPlugboardWire_Throws_WhenConstruct()
        => Should.Throw<InvalidOperationException>(() => new PlugboardWire(Letter.A, Letter.A));

    [Fact]
    public void ValidPlugboardWire_TransformsAllInputsToAllOutputs_Successfully()
    {
        var permutations = Letter
            .Cache
            .SelectMany(a => Letter.Cache.Where(b => a != b).Select(b => new PlugboardWire(a, b)));

        foreach (var wire in permutations)
        {
            foreach (var letter in Letter.Cache)
            {
                var actual = letter;
                var expected = wire.EndA == letter ?
                    wire.EndB :
                    wire.EndB == letter ?
                        wire.EndA :
                        letter;

                wire.Process(ref actual);
                actual.ShouldBe(expected);
            }
        }
    }
}