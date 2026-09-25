# Flight Data Decoder

[![CI](https://github.com/ScottMichel/flight-data-decoder/actions/workflows/ci.yml/badge.svg)](https://github.com/ScottMichel/flight-data-decoder/actions/workflows/ci.yml)

Un petit décodeur de données de vol, écrit en **C# / .NET 10**.

Pendant un vol, l'avion enregistre ses paramètres (altitude, vitesse, cap…) dans un fichier binaire, au format **ARINC 717**. Ce projet lit ce fichier et le transforme en valeurs lisibles, accessibles par une API REST.

> ⚠️ **Projet d'apprentissage** : les données sont **synthétiques** et le plan de lecture est **fictif**. Les vrais plans sont propriétaires et propres à chaque avion.

## Lancer le projet

Prérequis : le [SDK .NET 10](https://dotnet.microsoft.com/download).

```bash
# Lancer les tests
dotnet test

# Lancer l'API sur http://localhost:5080
dotnet run --project src/FlightDataDecoder.Api
```

Un vol de démonstration de 30 minutes est fourni : `samples/demo-flight.dat`. Pour le décoder, ouvre `src/FlightDataDecoder.Api/FlightDataDecoder.Api.http` dans VS Code (extension REST Client) et clique sur « Send Request ».

## Comment ça marche

```
fichier .dat  ──►  nombres de 12 bits  ──►  secondes de vol  ──►  valeurs physiques
```

1. Le fichier est une suite de nombres de 12 bits.
2. Des nombres repères (« mots de synchro ») marquent le début de chaque seconde de vol.
3. Un plan de lecture (`layouts/demo-layout.json`) indique où se trouve chaque paramètre et comment le convertir. Exemple : altitude brute 1250 × 8 = 10 000 ft.

Pour aller plus loin : [`docs/arinc717.md`](docs/arinc717.md).

## Organisation

| Dossier | Rôle |
|---|---|
| `src/FlightDataDecoder.Core` | La logique de décodage |
| `src/FlightDataDecoder.Api` | L'API REST |
| `src/FlightDataDecoder.Generator` | Génère des vols synthétiques |
| `tests/` | Les tests |
