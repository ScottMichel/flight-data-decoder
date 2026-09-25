using FlightDataDecoder.Core.Arinc717;

namespace FlightDataDecoder.Core.Tests;

public class FrameSynchronizerTests
{
    private const int WordsPerSubframe = 8;

    /// <summary>Construit un flux : des mots parasites, puis des sous-trames à partir d'une position de trame donnée.</summary>
    private static ushort[] BuildStream(int garbageWords, int subframeCount, int firstSyncIndex = 0)
    {
        var words = new List<ushort>();
        words.AddRange(Enumerable.Repeat((ushort)0x001, garbageWords));

        for (var i = 0; i < subframeCount; i++)
        {
            var subframe = new ushort[WordsPerSubframe];
            subframe[0] = Arinc717Constants.SyncWords[(firstSyncIndex + i) % 4];
            words.AddRange(subframe);
        }

        return [.. words];
    }

    [Fact]
    public void Synchronize_FindsAllSubframes_AfterLeadingGarbage()
    {
        var words = BuildStream(garbageWords: 3, subframeCount: 8);

        var subframes = FrameSynchronizer.Synchronize(words, WordsPerSubframe);

        Assert.Equal(8, subframes.Count);
        Assert.Equal(1, subframes[0].Number);
        Assert.Equal(4, subframes[3].Number);
    }

    [Fact]
    public void Synchronize_CanStartInTheMiddleOfAFrame()
    {
        var words = BuildStream(garbageWords: 0, subframeCount: 4, firstSyncIndex: 2);

        var subframes = FrameSynchronizer.Synchronize(words, WordsPerSubframe);

        Assert.Equal(3, subframes[0].Number);
        Assert.Equal(0, subframes[0].Second);
    }

    [Fact]
    public void Synchronize_ReturnsEmpty_WhenThereIsNoSyncWord()
    {
        var words = new ushort[100];

        Assert.Empty(FrameSynchronizer.Synchronize(words, WordsPerSubframe));
    }

    [Fact]
    public void Synchronize_Stops_WhenSyncIsLost()
    {
        var words = BuildStream(garbageWords: 0, subframeCount: 6);
        words[4 * WordsPerSubframe] = 0x000; // on casse la synchro de la 5e sous-trame

        var subframes = FrameSynchronizer.Synchronize(words, WordsPerSubframe);

        Assert.Equal(4, subframes.Count);
    }
}
