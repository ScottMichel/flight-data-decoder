namespace FlightDataDecoder.Core.Arinc717;

/// <summary>
/// Constantes de la norme ARINC 717 utilisées par le décodeur.
/// </summary>
public static class Arinc717Constants
{
    /// <summary>Un mot ARINC 717 fait 12 bits : on garde les 12 bits de poids faible.</summary>
    public const ushort WordMask = 0x0FFF;

    /// <summary>Nombre de sous-trames (donc de secondes) dans une trame.</summary>
    public const int SubframesPerFrame = 4;

    /// <summary>Mots de synchronisation placés au début des sous-trames 1, 2, 3 et 4.</summary>
    public static readonly IReadOnlyList<ushort> SyncWords = [0x247, 0x5B8, 0xA47, 0xDB8];
}
