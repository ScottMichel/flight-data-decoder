# CLAUDE.md — contexte du projet pour Claude Code

## Qui je suis et pourquoi ce projet existe

Je m'appelle Scott, développeur full stack TypeScript (React, Next.js, Node.js, Express, PostgreSQL, Prisma) avec 6 ans d'expérience. **J'apprends le C# / .NET avec ce projet.** Il sert aussi de vitrine GitHub pour une candidature à un poste de développeur full stack C# / React sur l'analyse de données de vol.

**Comment m'aider :**
- Explique chaque changement avant de le faire, en faisant le parallèle avec TypeScript / Node quand c'est utile.
- Avance par petites étapes, une fonctionnalité à la fois, avec un commit par étape.
- Quand c'est pédagogique, propose-moi d'écrire le code moi-même et relis-le ensuite.
- Ne réécris pas ce qui fonctionne sans raison ; le code doit rester simple et lisible pour un dev qui découvre le repo.
- Je travaille entièrement dans VS Code sous Windows : terminal intégré (PowerShell), panneau Contrôle de code source pour Git, panneau Testing (C# Dev Kit) pour les tests, extension REST Client pour l'API. Donne tes instructions dans ce cadre.

## Ce que fait le projet

Mini décodeur de données de vol au format ARINC 717 : fichier binaire → mots de 12 bits → sous-trames synchronisées → paramètres décodés (valeurs physiques) → API REST. Les données sont **synthétiques** et le layout est **fictif** (les vrais layouts sont propriétaires) : c'est assumé et écrit dans le README.

## Architecture

- `src/FlightDataDecoder.Core` : logique pure, aucune dépendance web. Dossiers :
  - `Arinc717/` : `Arinc717Constants`, `WordReader`, `Subframe`, `FrameSynchronizer`
  - `Layout/` : `FrameLayout`, `ParameterDefinition`, `ParameterEncoding`, `LayoutLoader` (JSON)
  - `Decoding/` : `BitExtractor`, `ParameterDecoder`, `FlightDecoder` (point d'entrée), `DecodedFlight`
  - `Writing/` : `ParameterEncoder`, `FrameWriter` (encodage inverse, pour le générateur et les tests)
- `src/FlightDataDecoder.Api` : minimal API ASP.NET Core, port 5080, requêtes de test dans `FlightDataDecoder.Api.http`, CORS ouvert pour `http://localhost:5173`. Routes : `GET /api/health`, `GET /api/layout`, `POST /api/decode` (multipart, champ `file`).
- `src/FlightDataDecoder.Generator` : console qui génère un vol synthétique (`FlightSimulator`).
- `tests/FlightDataDecoder.Core.Tests` : xUnit, tests unitaires + tests aller-retour (encode puis décode).
- `layouts/demo-layout.json` : 64 mots/s ; Altitude, Airspeed, Heading, Pitch (signé), GearDown (discret), EngineN1 (sous-trame 1 seulement).

## Décisions de format

- Chaque mot de 12 bits est stocké sur 16 bits little-endian ; 4 bits de poids fort ignorés.
- Mots de synchro : `0x247`, `0x5B8`, `0xA47`, `0xDB8` (sous-trames 1 à 4). Synchro validée par double confirmation.
- Numérotation ARINC : mot 1 = synchro ; bit 1 = poids faible.
- BNR : `valeur = brut × résolution + offset`, complément à deux si `signed`.
- Si la synchro est perdue, le décodeur s'arrête (resynchronisation = amélioration prévue).

## Conventions

- .NET 10, `Nullable` activé, `TreatWarningsAsErrors` (voir `Directory.Build.props`).
- Identifiants en anglais, commentaires XML et messages d'erreur en français.
- `record` pour les données, classes `static` sans état pour la logique du Core.
- Toute modification du Core est accompagnée de tests. Noms de tests : `Methode_Resultat_QuandCondition`.
- Pas de nombres magiques : constantes nommées.
- Commits conventionnels en français : `feat:`, `fix:`, `test:`, `docs:`, `chore:`, `refactor:`.

## Commandes

```bash
dotnet build
dotnet test
dotnet run --project src/FlightDataDecoder.Generator -- layouts/demo-layout.json samples/demo-flight.dat 1800
dotnet run --project src/FlightDataDecoder.Api
```

Pour tester l'API : `src/FlightDataDecoder.Api/FlightDataDecoder.Api.http` (extension REST Client, lien « Send Request »).

## Feuille de route

1. Front `web/` : React + TypeScript + Vite + **Redux Toolkit** (demandé par l'offre visée). Upload d'un fichier, appel à `/api/decode`, courbes par paramètre (Recharts), train d'atterrissage affiché comme un état.
2. Resynchronisation après perte de synchro, avec compteur de secondes perdues dans le résultat.
3. Codage BCD et superframes dans le layout.
4. Docker Compose (API + front) et mise à jour de la CI (build du front).
5. Optionnel : stockage des vols dans PostgreSQL avec EF Core.
