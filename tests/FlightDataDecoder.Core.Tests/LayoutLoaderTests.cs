using FlightDataDecoder.Core.Layout;

namespace FlightDataDecoder.Core.Tests;

public class LayoutLoaderTests
{
    [Fact]
    public void FromJson_ReadsParametersAndEnums()
    {
        const string json = """
            {
              "name": "Demo", "wordsPerSecond": 64,
              "parameters": [
                { "name": "GearDown", "unit": "", "word": 6, "firstBit": 1, "bitCount": 1, "encoding": "Discrete" }
              ]
            }
            """;

        var layout = LayoutLoader.FromJson(json);

        Assert.Equal(64, layout.WordsPerSecond);
        Assert.Equal(ParameterEncoding.Discrete, layout.Parameters[0].Encoding);
    }

    [Fact]
    public void FromJson_Throws_WhenParameterUsesTheSyncWord()
    {
        const string json = """
            { "name": "Bad", "wordsPerSecond": 64,
              "parameters": [ { "name": "X", "unit": "", "word": 1, "firstBit": 1, "bitCount": 12 } ] }
            """;

        Assert.Throws<InvalidDataException>(() => LayoutLoader.FromJson(json));
    }
}
