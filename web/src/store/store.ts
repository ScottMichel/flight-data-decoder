import { configureStore } from '@reduxjs/toolkit'
import flightReducer from './flightSlice'

/** Le store : l'unique source de vérité de l'application, composé de slices. */
export const store = configureStore({
  reducer: {
    flight: flightReducer,
  },
})

// Types déduits du store, pour que useSelector et useDispatch soient typés.
export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
