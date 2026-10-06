const SECONDS_PER_MINUTE = 60

/** Formate une durée en secondes : 1800 → "30 min 00 s". */
export function formatDuration(totalSeconds: number): string {
  const minutes = Math.floor(totalSeconds / SECONDS_PER_MINUTE)
  const seconds = totalSeconds % SECONDS_PER_MINUTE
  return `${minutes} min ${String(seconds).padStart(2, '0')} s`
}
