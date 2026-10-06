import FileUpload from './components/FileUpload'
import FlightSummary from './components/FlightSummary'
import { useAppSelector } from './store/hooks'
import './App.css'

function App() {
  const { status, fileName, flight, error } = useAppSelector((state) => state.flight)

  return (
    <main>
      <header className="header">
        <h1>Flight Data Decoder</h1>
        <p>Décodeur de données de vol ARINC 717 : envoyez un fichier binaire, l'API C# le décode.</p>
      </header>

      <FileUpload />

      {status === 'failed' && <p className="message error">{error}</p>}

      {status === 'succeeded' && flight && flight.durationSeconds === 0 && (
        <p className="message error">Aucune synchronisation trouvée : ce fichier ne semble pas être un flux ARINC 717.</p>
      )}

      {status === 'succeeded' && flight && flight.durationSeconds > 0 && (
        <FlightSummary fileName={fileName} flight={flight} />
      )}
    </main>
  )
}

export default App
