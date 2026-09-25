using FlightDataDecoder.Core.Layout;
using FlightDataDecoder.Core.Writing;
using FlightDataDecoder.Generator;

// Usage : dotnet run --project src/FlightDataDecoder.Generator -- <layout.json> <sortie.dat> [durée en secondes]
if (args.Length < 2)
{
    Console.Error.WriteLine("Usage : <layout.json> <sortie.dat> [durée en secondes, 1800 par défaut]");
    return 1;
}

var layoutPath = args[0];
var outputPath = args[1];
var durationSeconds = args.Length > 2 ? int.Parse(args[2]) : 1800;

var layout = LayoutLoader.FromFile(layoutPath);
var writer = new FrameWriter(layout);

// Quelques mots parasites au début : un vrai fichier ne commence pas forcément sur une synchro.
writer.WriteRawWords([0x123, 0x456, 0x789, 0xABC, 0x001]);

for (var second = 0; second < durationSeconds; second++)
{
    writer.WriteSecond(FlightSimulator.StateAt(second, durationSeconds));
}

var bytes = writer.ToBytes();
Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath))!);
File.WriteAllBytes(outputPath, bytes);

Console.WriteLine($"Vol de {durationSeconds} s généré : {outputPath} ({bytes.Length:N0} octets)");
return 0;
