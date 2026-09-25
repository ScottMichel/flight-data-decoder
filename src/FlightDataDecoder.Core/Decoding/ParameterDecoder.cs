using FlightDataDecoder.Core.Layout;

namespace FlightDataDecoder.Core.Decoding;

/// <summary>Convertit les bits d'un mot en valeur physique selon la définition du paramètre.</summary>
public static class ParameterDecoder
{
    public static double Decode(ushort word, ParameterDefinition parameter)
    {
        var raw = BitExtractor.Extract(word, parameter.FirstBit, parameter.BitCount);

        return parameter.Encoding switch
        {
            ParameterEncoding.Discrete => raw == 0 ? 0 : 1,
            ParameterEncoding.Bnr => ToPhysical(raw, parameter),
            _ => throw new NotSupportedException($"Codage non supporté : {parameter.Encoding}"),
        };
    }

    private static double ToPhysical(int raw, ParameterDefinition parameter)
    {
        var value = parameter.Signed ? BitExtractor.ToSigned(raw, parameter.BitCount) : raw;
        return value * parameter.Resolution + parameter.Offset;
    }
}
