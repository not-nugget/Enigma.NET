using Enigma.Machine;

using Shouldly;

namespace Enigma.Tests;

public class LetterTests
{
    [Fact]
    public void DefaultAlphabet_ReturnsTrue_WhenComparingToInvalid()
    {
        var a = new Letter();
        var e = Letter.Invalid;
        
        a.ShouldBe(e);
    }
}