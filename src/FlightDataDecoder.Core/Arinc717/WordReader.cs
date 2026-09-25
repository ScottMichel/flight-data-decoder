using System.Buffers.Binary;

namespace FlightDataDecoder.Core.Arinc717;

/// <summary>
/// Transforme les octets bruts d'un fichier en mots ARINC 717 de 12 bits.
/// </summary>
/// <remarks>
/// Format de fichier retenu : chaque mot de 12 bits est stocké dans un conteneur
/// de 16 bits little-endian. Les 4 bits de poids fort sont ignorés.
/// </remarks>
public static class WordReader
{
    private const int BytesPerWord = 2;

    public static ushort[] ReadWords(ReadOnlySpan<byte> data)
    {
        if (data.Length % BytesPerWord != 0)
        {
            throw new ArgumentException(
                $"La taille du fichier ({data.Length} octets) doit être un multiple de {BytesPerWord}.",
                nameof(data));
        }

        var words = new ushort[data.Length / BytesPerWord];

        for (var i = 0; i < words.Length; i++)
        {
            var container = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(i * BytesPerWord, BytesPerWord));
            words[i] = (ushort)(container & Arinc717Constants.WordMask);
        }

        return words;
    }
}
