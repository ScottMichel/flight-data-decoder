import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { Provider } from 'react-redux'
import App from './App.tsx'
import { store } from './store/store'
import './index.css'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    {/* Le Provider rend le store accessible à tous les composants, comme un Context.Provider. */}
    <Provider store={store}>
      <App />
    </Provider>
  </StrictMode>,
)
