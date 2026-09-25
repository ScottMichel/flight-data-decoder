namespace FlightDataDecoder.Core.Arinc717;

/// <summary>
/// Une sous-trame synchronisée, c'est-à-dire une seconde de vol.
/// </summary>
/// <param name="Second">Numéro de la seconde depuis la première synchronisation (0, 1, 2…).</param>
/// <param name="Number">Position dans la trame : 1, 2, 3 ou 4.</param>
/// <param name="Words">Les mots de la sous-trame, mot de synchro compris (index 0 = mot 1).</param>
public sealed record Subframe(int Second, int Number, ushort[] Words)
{
    /// <summary>Lit un mot par sa position ARINC (1 = mot de synchro).</summary>
    public ushort GetWord(int position) => Words[position - 1];
}
