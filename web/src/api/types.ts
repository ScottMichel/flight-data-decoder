// Ces types reflètent les records C# de FlightDataDecoder.Core/Decoding/DecodedFlight.cs.
// ASP.NET Core sérialise les propriétés en camelCase : Time → time, Value → value.

/** Bnr = un nombre (altitude…), Discrete = un état vrai/faux (train sorti…). */
export type ParameterEncoding = 'Bnr' | 'Discrete'

/** Une mesure : un instant (en secondes) et une valeur physique. */
export interface DataPoint {
  time: number
  value: number
}

/** Toutes les mesures d'un paramètre au cours du vol. */
export interface ParameterSeries {
  name: string
  unit: string
  encoding: ParameterEncoding
  points: DataPoint[]
}

/** Résultat complet du décodage d'un fichier. */
export interface DecodedFlight {
  layoutName: string
  durationSeconds: number
  parameters: ParameterSeries[]
}
