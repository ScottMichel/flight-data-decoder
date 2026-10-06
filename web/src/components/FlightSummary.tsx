import type { DecodedFlight } from '../api/types'
import { formatDuration } from '../utils/format'

interface FlightSummaryProps {
  fileName: string | null
  flight: DecodedFlight
}

/** Informations générales sur le vol décodé. */
function FlightSummary({ fileName, flight }: FlightSummaryProps) {
  return (
    <dl className="summary">
      <div>
        <dt>Fichier</dt>
        <dd>{fileName}</dd>
      </div>
      <div>
        <dt>Layout</dt>
        <dd>{flight.layoutName}</dd>
      </div>
      <div>
        <dt>Durée décodée</dt>
        <dd>{formatDuration(flight.durationSeconds)}</dd>
      </div>
      <div>
        <dt>Paramètres</dt>
        <dd>{flight.parameters.length}</dd>
      </div>
    </dl>
  )
}

export default FlightSummary
