import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // En développement, les appels à /api/... sont transmis à l'API C# (dotnet run).
    proxy: {
      '/api': 'http://localhost:5080',
    },
  },
})
