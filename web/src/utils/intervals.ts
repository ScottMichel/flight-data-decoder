import type { DataPoint } from '../api/types'

/** Une période pendant laquelle un paramètre discret garde la même valeur. */
export interface StateInterval {
  start: number
  /** Instant de la dernière mesure de la période, inclus. */
  end: number
  isOn: boolean
}

/**
 * Regroupe les mesures consécutives de même état en périodes.
 * Exemple : 1,1,0,0,0,1 → [sorti 0-1], [rentré 2-4], [sorti 5-5].
 */
export function toStateIntervals(points: DataPoint[]): StateInterval[] {
  const intervals: StateInterval[] = []

  for (const point of points) {
    const isOn = point.value !== 0
    const current = intervals.at(-1)

    if (current && current.isOn === isOn) {
      current.end = point.time
    } else {
      intervals.push({ start: point.time, end: point.time, isOn })
    }
  }

  return intervals
}
