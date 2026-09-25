using FlightDataDecoder.Core.Layout;

namespace FlightDataDecoder.Core.Writing;

/// <summary>
/// Opération inverse du décodage : place une valeur physique dans les bits d'un mot.
/// Sert au générateur de vols synthétiques et aux tests.
/// </summary>
public static class ParameterEncoder
{
    public static ushort Encode(ushort word, double value, ParameterDefinition parameter)
    {
        var raw = parameter.Encoding == ParameterEncoding.Discrete
            ? (value != 0 ? 1 : 0)
            : ToRaw(value, parameter);

        var mask = (1 << parameter.BitCount) - 1;
        var shift = parameter.FirstBit - 1;

        var cleared = word & ~(mask << shift);
        return (ushort)(cleared | ((raw & mask) << shift));
    }

    private static int ToRaw(double value, ParameterDefinition parameter)
    {
        var raw = (int)Math.Round((value - parameter.Offset) / parameter.Resolution);

        var min = parameter.Signed ? -(1 << (parameter.BitCount - 1)) : 0;
        var max = parameter.Signed ? (1 << (parameter.BitCount - 1)) - 1 : (1 << parameter.BitCount) - 1;

        return Math.Clamp(raw, min, max);
    }
}
