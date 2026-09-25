using FlightDataDecoder.Core.Decoding;
using FlightDataDecoder.Core.Layout;
using FlightDataDecoder.Core.Writing;

namespace FlightDataDecoder.Core.Tests;

/// <summary>
/// Tests « aller-retour » : on écrit des valeurs connues avec le FrameWriter,
/// puis on vérifie que le décodeur retrouve exactement les mêmes.
/// </summary>
public class FlightDecoderTests
{
    private static readonly FrameLayout Layout = new("Test", WordsPerSecond: 16,
    [
        new ParameterDefinition("Altitude", "ft", Word: 2, FirstBit: 1, BitCount: 12, Resolution: 8),
        new ParameterDefinition("EngineN1", "%", Word: 3, FirstBit: 1, BitCount: 12, Resolution: 0.05, Subframes: [1]),
    ]);

    private static byte[] WriteFlight(int seconds)
    {
        var writer = new FrameWriter(Layout);
        writer.WriteRawWords([0x123, 0x456]);

        for (var second = 0; second < seconds; second++)
        {
            writer.WriteSecond(new Dictionary<string, double>
            {
                ["Altitude"] = second * 1000,
                ["EngineN1"] = 85,
            });
        }

        return writer.ToBytes();
    }

    [Fact]
    public void Decode_RoundTrip_ReturnsTheWrittenValues()
    {
        var flight = FlightDecoder.Decode(WriteFlight(seconds: 8), Layout);

        var altitude = flight.Parameters.Single(p => p.Name == "Altitude");
        Assert.Equal(8, flight.DurationSeconds);
        Assert.Equal(new double[] { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000 }, altitude.Points.Select(p => p.Value));
    }

    [Fact]
    public void Decode_ReadsSlowParameters_OnlyInTheirSubframes()
    {
        var flight = FlightDecoder.Decode(WriteFlight(seconds: 8), Layout);

        var n1 = flight.Parameters.Single(p => p.Name == "EngineN1");
        Assert.Equal(new[] { 0, 4 }, n1.Points.Select(p => p.Time));
        Assert.All(n1.Points, point => Assert.Equal(85, point.Value, precision: 6));
    }
}
