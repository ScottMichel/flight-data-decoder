import { describe, expect, it } from 'vitest'
import type { DecodedFlight } from '../api/types'
import flightReducer, { decodeFile, reset } from './flightSlice'

const file = new File([new Uint8Array(4)], 'vol.dat')
const flight: DecodedFlight = { layoutName: 'Demo', durationSeconds: 0, parameters: [] }

// Un reducer est une fonction pure (état, action) → nouvel état : on le teste sans React ni serveur.
describe('flightSlice', () => {
  it('passe en chargement et retient le nom du fichier quand le décodage démarre', () => {
    const state = flightReducer(undefined, decodeFile.pending('id', file))

    expect(state.status).toBe('loading')
    expect(state.fileName).toBe('vol.dat')
  })

  it('stocke le vol quand le décodage réussit', () => {
    const state = flightReducer(undefined, decodeFile.fulfilled(flight, 'id', file))

    expect(state.status).toBe('succeeded')
    expect(state.flight).toEqual(flight)
  })

  it("stocke le message d'erreur quand le décodage échoue", () => {
    const state = flightReducer(undefined, decodeFile.rejected(new Error('Fichier invalide'), 'id', file))

    expect(state.status).toBe('failed')
    expect(state.error).toBe('Fichier invalide')
  })

  it("revient à l'état initial avec reset", () => {
    const decoded = flightReducer(undefined, decodeFile.fulfilled(flight, 'id', file))

    expect(flightReducer(decoded, reset()).status).toBe('idle')
  })
})
