using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlightDataDecoder.Core.Layout;

/// <summary>Charge et valide un <see cref="FrameLayout"/> décrit en JSON.</summary>
public static class LayoutLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public static FrameLayout FromFile(string path) => FromJson(File.ReadAllText(path));

    public static FrameLayout FromJson(string json)
    {
        var layout = JsonSerializer.Deserialize<FrameLayout>(json, JsonOptions)
                     ?? throw new InvalidDataException("Le fichier de layout est vide.");

        Validate(layout);
        return layout;
    }

    private static void Validate(FrameLayout layout)
    {
        foreach (var parameter in layout.Parameters)
        {
            if (parameter.Word < 2 || parameter.Word > layout.WordsPerSecond)
            {
                throw new InvalidDataException(
                    $"{parameter.Name} : le mot {parameter.Word} doit être entre 2 et {layout.WordsPerSecond}.");
            }

            if (parameter.FirstBit < 1 || parameter.BitCount < 1 || parameter.FirstBit + parameter.BitCount - 1 > 12)
            {
                throw new InvalidDataException($"{parameter.Name} : les bits doivent tenir entre 1 et 12.");
            }
        }
    }
}
