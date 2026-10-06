import type { ParameterSeries } from '../api/types'
import { formatClock } from '../utils/format'
import { toStateIntervals } from '../utils/intervals'

/** Libellés des deux états. Pour le train : 1 = sorti, 0 = rentré. */
const ON_LABEL = 'Sorti'
const OFF_LABEL = 'Rentré'

interface StateTimelineProps {
  series: ParameterSeries
  durationSeconds: number
}

/** Paramètre discret (ex. train d'atterrissage) affiché comme un état sur la ligne de temps. */
function StateTimeline({ series, durationSeconds }: StateTimelineProps) {
  const intervals = toStateIntervals(series.points)
  // Chaque mesure couvre une seconde : la période [start, end] dure end - start + 1 secondes.
  const widthPercent = (start: number, end: number) => ((end - start + 1) / durationSeconds) * 100

  return (
    <section className="card">
      <h2 className="chart-title">{series.name}</h2>

      <div className="timeline" role="img" aria-label={`${series.name} au cours du vol`}>
        {intervals.map((interval) => {
          const label = interval.isOn ? ON_LABEL : OFF_LABEL
          return (
            <div
              key={interval.start}
              className={`timeline-segment ${interval.isOn ? 'on' : 'off'}`}
              style={{ width: `${widthPercent(interval.start, interval.end)}%` }}
              title={`${label} de ${formatClock(interval.start)} à ${formatClock(interval.end)}`}
            >
              <span>{label}</span>
            </div>
          )
        })}
      </div>

      <ul className="timeline-events">
        {intervals.map((interval) => (
          <li key={interval.start}>
            <strong>{formatClock(interval.start)}</strong> {interval.isOn ? ON_LABEL : OFF_LABEL}
          </li>
        ))}
      </ul>
    </section>
  )
}

export default StateTimeline
