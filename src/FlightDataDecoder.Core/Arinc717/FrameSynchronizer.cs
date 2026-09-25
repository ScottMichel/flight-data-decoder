namespace FlightDataDecoder.Core.Arinc717;

/// <summary>
/// Retrouve les sous-trames dans un flux de mots grâce aux mots de synchronisation.
/// </summary>
public static class FrameSynchronizer
{
    /// <summary>
    /// Découpe le flux en sous-trames synchronisées.
    /// </summary>
    /// <remarks>
    /// 1. On cherche une position où un mot de synchro est suivi, exactement
    ///    une sous-trame plus loin, du mot de synchro suivant (double confirmation).
    /// 2. À partir de là, on avance d'une sous-trame à la fois tant que la synchro est correcte.
    /// Version simple : si la synchro est perdue, on s'arrête.
    /// </remarks>
    public static IReadOnlyList<Subframe> Synchronize(ushort[] words, int wordsPerSubframe)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(wordsPerSubframe, 2);

        var start = FindFirstSync(words, wordsPerSubframe);
        if (start is null)
        {
            return [];
        }

        var subframes = new List<Subframe>();
        var (position, syncIndex) = start.Value;

        while (position + wordsPerSubframe <= words.Length
               && words[position] == Arinc717Constants.SyncWords[syncIndex])
        {
            var subframeWords = words.AsSpan(position, wordsPerSubframe).ToArray();
            subframes.Add(new Subframe(subframes.Count, syncIndex + 1, subframeWords));

            position += wordsPerSubframe;
            syncIndex = (syncIndex + 1) % Arinc717Constants.SubframesPerFrame;
        }

        return subframes;
    }

    private static (int Position, int SyncIndex)? FindFirstSync(ushort[] words, int wordsPerSubframe)
    {
        var syncWords = Arinc717Constants.SyncWords;

        for (var position = 0; position + wordsPerSubframe < words.Length; position++)
        {
            for (var syncIndex = 0; syncIndex < syncWords.Count; syncIndex++)
            {
                var nextSyncIndex = (syncIndex + 1) % Arinc717Constants.SubframesPerFrame;

                if (words[position] == syncWords[syncIndex]
                    && words[position + wordsPerSubframe] == syncWords[nextSyncIndex])
                {
                    return (position, syncIndex);
                }
            }
        }

        return null;
    }
}
