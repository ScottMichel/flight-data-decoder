using FlightDataDecoder.Core.Arinc717;

namespace FlightDataDecoder.Core.Tests;

public class WordReaderTests
{
    [Fact]
    public void ReadWords_KeepsOnlyThe12LowBits()
    {
        // 0xF247 en little-endian : l'octet de poids faible (0x47) vient en premier.
        byte[] data = [0x47, 0xF2];

        var words = WordReader.ReadWords(data);

        Assert.Equal(new ushort[] { 0x247 }, words);
    }

    [Fact]
    public void ReadWords_Throws_WhenLengthIsOdd()
    {
        byte[] data = [0x01, 0x02, 0x03];

        Assert.Throws<ArgumentException>(() => WordReader.ReadWords(data));
    }
}
