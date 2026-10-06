import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'
import { decodeFlight } from '../api/flightApi'
import type { DecodedFlight } from '../api/types'

// Un « slice » regroupe une partie de l'état global et les fonctions qui le modifient.
// Équivalent : un useState partagé via un Context, mais avec des transitions nommées et traçables.

type Status = 'idle' | 'loading' | 'succeeded' | 'failed'

interface FlightState {
  status: Status
  fileName: string | null
  flight: DecodedFlight | null
  error: string | null
}

const initialState: FlightState = {
  status: 'idle',
  fileName: null,
  flight: null,
  error: null,
}

/**
 * Action asynchrone : appelle l'API puis déclenche automatiquement
 * decodeFile.pending, puis decodeFile.fulfilled ou decodeFile.rejected.
 */
export const decodeFile = createAsyncThunk('flight/decodeFile', (file: File) => decodeFlight(file))

const flightSlice = createSlice({
  name: 'flight',
  initialState,
  reducers: {
    /** Revient à l'écran d'accueil. */
    reset: () => initialState,
  },
  // Réactions aux trois étapes de l'appel à l'API.
  // Redux Toolkit utilise Immer : on peut écrire « state.x = ... » sans muter réellement l'état.
  extraReducers: (builder) => {
    builder
      .addCase(decodeFile.pending, (state, action) => {
        state.status = 'loading'
        state.fileName = action.meta.arg.name
        state.flight = null
        state.error = null
      })
      .addCase(decodeFile.fulfilled, (state, action) => {
        state.status = 'succeeded'
        state.flight = action.payload
      })
      .addCase(decodeFile.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message ?? 'Erreur inconnue.'
      })
  },
})

export const { reset } = flightSlice.actions
export default flightSlice.reducer
