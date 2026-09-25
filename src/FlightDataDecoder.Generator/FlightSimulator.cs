namespace FlightDataDecoder.Generator;

/// <summary>
/// Simule un vol simplifié, seconde par seconde : roulage, décollage, montée,
/// croisière, descente et atterrissage. Les valeurs sont plausibles, pas réalistes au sens aéronautique.
/// </summary>
public static class FlightSimulator
{
    private const double CruiseAltitude = 30_000;

    public static Dictionary<string, double> StateAt(int second, int totalSeconds)
    {
        // Chaque phase est exprimée en fraction de la durée totale du vol.
        var progress = (double)second / totalSeconds;

        var (altitude, airspeed, pitch, n1) = progress switch
        {
            < 0.05 => (0.0, 15.0, 0.0, 25.0),                                         // roulage
            < 0.08 => (0.0, Lerp(15, 160, Phase(progress, 0.05, 0.08)), 0.0, 90.0),   // course au décollage
            < 0.45 => (Lerp(0, CruiseAltitude, Phase(progress, 0.08, 0.45)), 280.0, 8.0, 88.0), // montée
            < 0.70 => (CruiseAltitude, 450.0, 2.5, 80.0),                             // croisière
            < 0.95 => (Lerp(CruiseAltitude, 0, Phase(progress, 0.70, 0.95)), 250.0, -3.0, 45.0), // descente
            _ => (0.0, Lerp(140, 20, Phase(progress, 0.95, 1.0)), 0.0, 30.0),         // atterrissage
        };

        var gearDown = progress < 0.09 || progress > 0.90 ? 1.0 : 0.0;
        var heading = (90 + 40 * Math.Sin(progress * Math.PI * 2)) % 360;

        return new Dictionary<string, double>
        {
            ["Altitude"] = altitude,
            ["Airspeed"] = airspeed,
            ["Heading"] = heading,
            ["Pitch"] = pitch,
            ["GearDown"] = gearDown,
            ["EngineN1"] = n1,
        };
    }

    private static double Phase(double progress, double start, double end) => (progress - start) / (end - start);

    private static double Lerp(double from, double to, double t) => from + (to - from) * t;
}
