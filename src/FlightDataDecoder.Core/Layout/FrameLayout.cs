namespace FlightDataDecoder.Core.Layout;

/// <summary>
/// Le « plan de lecture » d'un flux ARINC 717 : taille des sous-trames et liste des paramètres.
/// </summary>
public sealed record FrameLayout(string Name, int WordsPerSecond, IReadOnlyList<ParameterDefinition> Parameters);
