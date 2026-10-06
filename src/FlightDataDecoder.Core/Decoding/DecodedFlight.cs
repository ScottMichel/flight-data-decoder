using FlightDataDecoder.Core.Layout;

namespace FlightDataDecoder.Core.Decoding;

/// <summary>Une mesure : un instant (en secondes) et une valeur physique.</summary>
public sealed record DataPoint(int Time, double Value);

/// <summary>Toutes les mesures d'un paramètre au cours du vol.</summary>
/// <param name="Encoding">Codage du paramètre : permet au front d'afficher un discret comme un état.</param>
public sealed record ParameterSeries(string Name, string Unit, ParameterEncoding Encoding, IReadOnlyList<DataPoint> Points);

/// <summary>Résultat complet du décodage d'un fichier.</summary>
/// <param name="LayoutName">Nom du layout utilisé.</param>
/// <param name="DurationSeconds">Nombre de secondes synchronisées (une sous-trame = une seconde).</param>
/// <param name="Parameters">Séries décodées, une par paramètre du layout.</param>
public sealed record DecodedFlight(string LayoutName, int DurationSeconds, IReadOnlyList<ParameterSeries> Parameters);
