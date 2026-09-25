using System.Buffers.Binary;
using FlightDataDecoder.Core.Arinc717;
using FlightDataDecoder.Core.Layout;

namespace FlightDataDecoder.Core.Writing;

/// <summary>
/// Construit un flux ARINC 717 binaire à partir de valeurs physiques, seconde par seconde.
/// </summary>
public sealed class FrameWriter(FrameLayout layout)
{
    private readonly List<ushort> _words = [];
    private int _secondsWritten;

    /// <summary>Ajoute des mots quelconques (pour simuler un fichier qui ne commence pas sur une synchro).</summary>
    public void WriteRawWords(IEnumerable<ushort> words) =>
        _words.AddRange(words.Select(word => (ushort)(word & Arinc717Constants.WordMask)));

    /// <summary>Écrit une sous-trame (une seconde) avec les valeurs fournies, indexées par nom de paramètre.</summary>
    public void WriteSecond(IReadOnlyDictionary<string, double> values)
    {
        var subframeIndex = _secondsWritten % Arinc717Constants.SubframesPerFrame;
        var subframe = new ushort[layout.WordsPerSecond];
        subframe[0] = Arinc717Constants.SyncWords[subframeIndex];

        foreach (var parameter in layout.Parameters.Where(p => p.IsInSubframe(subframeIndex + 1)))
        {
            if (values.TryGetValue(parameter.Name, out var value))
            {
                var index = parameter.Word - 1;
                subframe[index] = ParameterEncoder.Encode(subframe[index], value, parameter);
            }
        }

        _words.AddRange(subframe);
        _secondsWritten++;
    }

    /// <summary>Renvoie le flux au format fichier : chaque mot sur 16 bits little-endian.</summary>
    public byte[] ToBytes()
    {
        var bytes = new byte[_words.Count * 2];
        for (var i = 0; i < _words.Count; i++)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(bytes.AsSpan(i * 2, 2), _words[i]);
        }

        return bytes;
    }
}
