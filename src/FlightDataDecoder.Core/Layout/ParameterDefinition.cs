namespace FlightDataDecoder.Core.Layout;

/// <summary>
/// Décrit où trouver un paramètre dans une sous-trame et comment le convertir.
/// </summary>
/// <param name="Name">Nom du paramètre (ex. "Altitude").</param>
/// <param name="Unit">Unité physique (ex. "ft").</param>
/// <param name="Word">Position du mot dans la sous-trame (1 = mot de synchro, donc 2 minimum).</param>
/// <param name="FirstBit">Premier bit utilisé, 1 = bit de poids faible.</param>
/// <param name="BitCount">Nombre de bits utilisés.</param>
/// <param name="Encoding">Type de codage.</param>
/// <param name="Signed">Vrai si la valeur est signée (complément à deux).</param>
/// <param name="Resolution">Valeur physique d'une unité brute (valeur = brut × résolution + offset).</param>
/// <param name="Offset">Décalage ajouté après la multiplication.</param>
/// <param name="Subframes">Sous-trames qui contiennent le paramètre. Vide = toutes.</param>
public sealed record ParameterDefinition(
    string Name,
    string Unit,
    int Word,
    int FirstBit,
    int BitCount,
    ParameterEncoding Encoding = ParameterEncoding.Bnr,
    bool Signed = false,
    double Resolution = 1,
    double Offset = 0,
    int[]? Subframes = null)
{
    /// <summary>Indique si le paramètre est présent dans la sous-trame donnée (1 à 4).</summary>
    public bool IsInSubframe(int subframeNumber) =>
        Subframes is null || Subframes.Length == 0 || Subframes.Contains(subframeNumber);
}
