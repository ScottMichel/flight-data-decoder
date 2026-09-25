using FlightDataDecoder.Core.Decoding;
using FlightDataDecoder.Core.Layout;

namespace FlightDataDecoder.Core.Tests;

public class ParameterDecoderTests
{
    [Fact]
    public void Decode_AppliesResolution()
    {
        var altitude = new ParameterDefinition("Altitude", "ft", Word: 2, FirstBit: 1, BitCount: 12, Resolution: 8);

        var value = ParameterDecoder.Decode(1250, altitude);

        Assert.Equal(10_000, value);
    }

    [Fact]
    public void Decode_ReturnsNegativeValue_WhenSignBitIsSet()
    {
        var pitch = new ParameterDefinition("Pitch", "deg", Word: 5, FirstBit: 1, BitCount: 12, Signed: true, Resolution: 0.1);

        // 0xFE2 = -30 en complément à deux sur 12 bits, soit -3,0 degrés.
        var value = ParameterDecoder.Decode(0xFE2, pitch);

        Assert.Equal(-3.0, value, precision: 6);
    }

    [Fact]
    public void Decode_ReturnsOne_WhenDiscreteBitIsSet()
    {
        var gear = new ParameterDefinition("GearDown", "", Word: 6, FirstBit: 1, BitCount: 1, Encoding: ParameterEncoding.Discrete);

        Assert.Equal(1, ParameterDecoder.Decode(0b1, gear));
        Assert.Equal(0, ParameterDecoder.Decode(0b10, gear));
    }
}
