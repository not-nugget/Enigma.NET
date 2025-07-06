using Enigma.Machine;

using Shouldly;

namespace Enigma.Tests;

public class AlphabetTests
{
    [Fact]
    public void DefaultAlphabet_ReturnsTrue_WhenComparingToInvalid()
    {
        var a = new Alphabet();
        var e = Alphabet.Invalid;
        
        a.ShouldBe(e);
    }
}