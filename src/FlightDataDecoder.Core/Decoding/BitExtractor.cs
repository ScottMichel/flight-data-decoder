namespace FlightDataDecoder.Core.Decoding;

/// <summary>Opérations sur les bits d'un mot de 12 bits.</summary>
public static class BitExtractor
{
    /// <summary>
    /// Extrait <paramref name="bitCount"/> bits à partir de <paramref name="firstBit"/> (1 = poids faible).
    /// </summary>
    /// <example>Extract(0b1011_0000, firstBit: 5, bitCount: 4) renvoie 0b1011 (11).</example>
    public static int Extract(ushort word, int firstBit, int bitCount)
    {
        var mask = (1 << bitCount) - 1;
        return (word >> (firstBit - 1)) & mask;
    }

    /// <summary>
    /// Interprète une valeur brute en complément à deux sur <paramref name="bitCount"/> bits.
    /// </summary>
    /// <example>Sur 12 bits : 0xFFF donne -1, 0x800 donne -2048.</example>
    public static int ToSigned(int raw, int bitCount)
    {
        var signBit = 1 << (bitCount - 1);
        return (raw & signBit) != 0 ? raw - (1 << bitCount) : raw;
    }
}
