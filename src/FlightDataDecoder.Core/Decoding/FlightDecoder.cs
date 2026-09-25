using FlightDataDecoder.Core.Arinc717;
using FlightDataDecoder.Core.Layout;

namespace FlightDataDecoder.Core.Decoding;

/// <summary>
/// Point d'entrée du Core : octets bruts → mots 12 bits → sous-trames → paramètres décodés.
/// </summary>
public static class FlightDecoder
{
    public static DecodedFlight Decode(ReadOnlySpan<byte> data, FrameLayout layout)
    {
        var words = WordReader.ReadWords(data);
        var subframes = FrameSynchronizer.Synchronize(words, layout.WordsPerSecond);

        var series = layout.Parameters
            .Select(parameter => DecodeParameter(subframes, parameter))
            .ToList();

        return new DecodedFlight(layout.Name, subframes.Count, series);
    }

    private static ParameterSeries DecodeParameter(IReadOnlyList<Subframe> subframes, ParameterDefinition parameter)
    {
        var points = subframes
            .Where(subframe => parameter.IsInSubframe(subframe.Number))
            .Select(subframe => new DataPoint(
                subframe.Second,
                ParameterDecoder.Decode(subframe.GetWord(parameter.Word), parameter)))
            .ToList();

        return new ParameterSeries(parameter.Name, parameter.Unit, points);
    }
}
