using FlightDataDecoder.Core.Decoding;

namespace FlightDataDecoder.Core.Tests;

public class BitExtractorTests
{
    [Theory]
    [InlineData(0b1011_0000, 5, 4, 0b1011)]
    [InlineData(0xFFF, 1, 12, 4095)]
    [InlineData(0b0000_0001, 1, 1, 1)]
    [InlineData(0b0000_0010, 1, 1, 0)]
    public void Extract_ReturnsTheRequestedBits(int word, int firstBit, int bitCount, int expected)
    {
        var raw = BitExtractor.Extract((ushort)word, firstBit, bitCount);

        Assert.Equal(expected, raw);
    }

    [Theory]
    [InlineData(0xFFF, 12, -1)]
    [InlineData(0x800, 12, -2048)]
    [InlineData(0x7FF, 12, 2047)]
    [InlineData(30, 12, 30)]
    public void ToSigned_InterpretsTwosComplement(int raw, int bitCount, int expected)
    {
        Assert.Equal(expected, BitExtractor.ToSigned(raw, bitCount));
    }
}
