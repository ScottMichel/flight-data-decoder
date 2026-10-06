import type { DecodedFlight } from './types'

/** Envoie un fichier .dat à l'API C# (POST /api/decode) et renvoie le vol décodé. */
export async function decodeFlight(file: File): Promise<DecodedFlight> {
  // Même nom de champ que côté C# : IFormFile file.
  const formData = new FormData()
  formData.append('file', file)

  const response = await fetch('/api/decode', { method: 'POST', body: formData })

  if (!response.ok) {
    // L'API renvoie { error: "..." } en cas de fichier invalide.
    const body = await response.json().catch(() => null)
    throw new Error(body?.error ?? `Erreur ${response.status} lors du décodage.`)
  }

  return response.json()
}
