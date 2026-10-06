import type { ChangeEvent } from 'react'
import { decodeFile } from '../store/flightSlice'
import { useAppDispatch, useAppSelector } from '../store/hooks'

/** Choix d'un fichier .dat : il est envoyé à l'API dès qu'il est sélectionné. */
function FileUpload() {
  const dispatch = useAppDispatch()
  const status = useAppSelector((state) => state.flight.status)

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0]
    if (file) {
      dispatch(decodeFile(file))
    }
    // Permet de re-sélectionner le même fichier.
    event.target.value = ''
  }

  return (
    <label className="upload">
      <input type="file" accept=".dat" onChange={handleChange} disabled={status === 'loading'} />
      <span className="upload-button">{status === 'loading' ? 'Décodage…' : 'Choisir un fichier .dat'}</span>
      <span className="upload-hint">Exemple fourni : samples/demo-flight.dat</span>
    </label>
  )
}

export default FileUpload
