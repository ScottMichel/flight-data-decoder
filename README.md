# Flight Data Decoder

Mini décodeur de données de vol au format **ARINC 717**, écrit en **C# / .NET 10**.

Il lit un fichier binaire enregistré « à bord », retrouve les secondes de vol grâce aux mots de synchronisation, extrait chaque paramètre (altitude, vitesse, cap…) et le convertit en valeur physique. Le résultat est exposé par une API REST.

> ⚠️ **Projet d'apprentissage.** Les vrais plans de lecture (frame layouts) sont propriétaires et propres à chaque avion. Ce projet utilise un **layout fictif** (`layouts/demo-layout.json`) et des **vols synthétiques** générés par l'outil `Generator`, qui respectent la structure ARINC 717 sans correspondre à un avion réel.

## Démarrage rapide

Prérequis : [SDK .NET 10](https://dotnet.microsoft.com/download).

```bash
# 1. Lancer les tests
dotnet test

# 2. Générer un vol synthétique de 30 minutes
dotnet run --project src/FlightDataDecoder.Generator -- layouts/demo-layout.json samples/demo-flight.dat 1800

# 3. Lancer l'API (http://localhost:5080)
dotnet run --project src/FlightDataDecoder.Api

# 4. Décoder le fichier (dans un autre terminal)
curl -F "file=@samples/demo-flight.dat" http://localhost:5080/api/decode
```

## Comment ça marche

```
fichier .dat ──► WordReader ──► FrameSynchronizer ──► FlightDecoder ──► JSON
 (octets)        (mots 12 bits)  (sous-trames = 1 s)   (+ layout JSON)   (séries de valeurs)
```

1. **WordReader** : chaque mot de 12 bits est stocké sur 16 bits little-endian ; on garde les 12 bits de poids faible.
2. **FrameSynchronizer** : cherche les mots de synchro `0x247`, `0x5B8`, `0xA47`, `0xDB8` qui ouvrent les 4 sous-trames d'une trame, même si le fichier commence au milieu d'une seconde.
3. **FlightDecoder** : pour chaque paramètre du layout, lit le bon mot, extrait les bons bits, gère le signe (complément à deux) et applique la résolution.

Plus de détails sur la norme dans [`docs/arinc717.md`](docs/arinc717.md).

## Organisation

| Dossier | Rôle |
|---|---|
| `src/FlightDataDecoder.Core` | Toute la logique de décodage, sans dépendance au web. |
| `src/FlightDataDecoder.Api` | API REST minimale qui expose le Core. |
| `src/FlightDataDecoder.Generator` | Outil console qui génère des vols synthétiques. |
| `tests/FlightDataDecoder.Core.Tests` | Tests unitaires xUnit du Core. |
| `layouts/` | Plans de lecture des paramètres (JSON). |
| `samples/` | Fichiers de vol générés. |

## API

| Méthode | Route | Description |
|---|---|---|
| `GET` | `/api/health` | Vérifie que l'API répond. |
| `GET` | `/api/layout` | Renvoie le layout utilisé. |
| `POST` | `/api/decode` | Décode un fichier envoyé en `multipart/form-data` (champ `file`). |

## Choix techniques

- **Core indépendant** : le décodage se teste sans lancer de serveur, et pourrait être réutilisé dans un autre outil.
- **Layout en JSON** : on ajoute un paramètre sans toucher au code.
- **Tests aller-retour** : le `FrameWriter` encode des valeurs connues, le décodeur doit les retrouver à l'identique.
- **Compilation stricte** : types nullables vérifiés et warnings traités comme des erreurs (`Directory.Build.props`).

## Pistes d'évolution

- Resynchronisation automatique après une perte de synchro.
- Front React + Redux Toolkit + Vite pour afficher les courbes.
- Codage BCD, superframes, Docker Compose.
