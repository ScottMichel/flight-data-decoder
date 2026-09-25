namespace FlightDataDecoder.Core.Layout;

/// <summary>Façon dont la valeur d'un paramètre est codée dans ses bits.</summary>
public enum ParameterEncoding
{
    /// <summary>Binary Number Representation : un nombre binaire, éventuellement signé.</summary>
    Bnr,

    /// <summary>Un état vrai/faux (ex. : train d'atterrissage sorti).</summary>
    Discrete,
}
