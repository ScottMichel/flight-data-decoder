import { CartesianGrid, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts'
import type { ParameterSeries } from '../api/types'
import { formatClock } from '../utils/format'

const CHART_HEIGHT = 180

/** Identifiant commun : survoler un graphique affiche le même instant sur tous les autres. */
const CHART_SYNC_ID = 'flight'

interface ParameterChartProps {
  series: ParameterSeries
}

/** Courbe d'un paramètre numérique (BNR) en fonction du temps. */
function ParameterChart({ series }: ParameterChartProps) {
  const unit = series.unit ? ` ${series.unit}` : ''

  return (
    <section className="card">
      <h2 className="chart-title">
        {series.name} <span className="chart-unit">{series.unit && `(${series.unit})`}</span>
      </h2>
      <ResponsiveContainer width="100%" height={CHART_HEIGHT}>
        {/* syncMethod="value" : on synchronise sur le temps, pas sur l'index du point
            (EngineN1 n'a qu'une mesure toutes les 4 secondes). */}
        <LineChart data={series.points} syncId={CHART_SYNC_ID} syncMethod="value" margin={{ top: 8, right: 16, bottom: 0, left: 0 }}>
          <CartesianGrid stroke="var(--border)" vertical={false} />
          <XAxis
            dataKey="time"
            type="number"
            domain={['dataMin', 'dataMax']}
            tickFormatter={formatClock}
            stroke="var(--muted)"
            fontSize={12}
          />
          <YAxis stroke="var(--muted)" fontSize={12} width={56} />
          <Tooltip
            labelFormatter={(time) => `t = ${formatClock(Number(time))}`}
            formatter={(value) => [`${Number(value).toFixed(1)}${unit}`, series.name]}
            contentStyle={{ background: 'var(--card)', border: '1px solid var(--border)', borderRadius: 6 }}
            labelStyle={{ color: 'var(--muted)' }}
            itemStyle={{ color: 'var(--text-h)' }}
          />
          {/* Pas d'animation ni de points : 1800 mesures s'affichent instantanément. */}
          <Line
            dataKey="value"
            type="linear"
            stroke="var(--accent)"
            strokeWidth={2}
            dot={false}
            activeDot={{ r: 4 }}
            isAnimationActive={false}
          />
        </LineChart>
      </ResponsiveContainer>
    </section>
  )
}

export default ParameterChart
