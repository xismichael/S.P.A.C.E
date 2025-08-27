using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class RatingSystem
{
    private const float TEMP_PENALTY_MAX = 45f;  // Big direct drop if temperature doesn’t fit
    private const float BIOME_PENALTY_MAX = 35f; // Big direct drop if biome mismatched

    private const float RAD_PENALTY_MAX = 20f;  // Radiation penalty is smaller than temp/biome
    private const float RAD_GAMMA = 2.0f; // Nonlinear: punishes large miss much more

    private const float ATMOS_BONUS_MAX = 20f;   // Pure bonus
    private const float LIFE_BONUS_MAX = 10f;   // Pure bonus
    private const float HZ_BONUS_MAX = 5f;   // Tiny nudge bonus

    /// <summary>
    /// Calculates the final match rating between a creature and a planet.
    /// - Temperature & Biome mismatches cause large direct point drops.
    /// - Radiation causes a nonlinear penalty (bigger miss = harsher).
    /// - Atmosphere, Lifespan, and Habitable Zone add small bonus points.
    /// Returns a value clamped between 0–100.
    /// </summary>
    public static float GetCreaturePlanetRating(Creature creature, Planet planet)
    {
        if (creature == null || planet == null || planet.conditions == null || creature.traits == null)
            return 0f;

        float r = Mathf.Clamp01(creature.traits.generalResilience);
        float score100 = 100f;

        // --- BIOME penalty ---
        {
            float s = ScoreListContainsToken(
                token: planet.conditions.planetType,
                allowed: creature.traits.biome,
                resilience: r,
                mismatchFloor: 0.05f,
                mismatchCeil: 0.6f
            );
            if (s >= 0f)
                score100 -= (1f - s) * BIOME_PENALTY_MAX;
        }

        // --- TEMPERATURE penalty ---
        {
            float s = ScoreTemperatureRangeContains(
                planetLower: planet.conditions.tempLower,
                planetUpper: planet.conditions.tempUpper,
                creatureLower: creature.traits.tempLower,
                creatureUpper: creature.traits.tempUpper,
                resilience: r
            );
            if (s >= 0f)
                score100 -= (1f - s) * TEMP_PENALTY_MAX;
        }

        // --- RADIATION penalty ---
        {
            float s = ScoreRadiationFromDistance(
                distanceFromStar: planet.conditions.distanceFromStar,
                radLower: creature.traits.radiationLower,
                radUpper: creature.traits.radiationUpper,
                resilience: r,
                referenceDistance: 1.0f
            );
            if (s >= 0f)
            {
                float miss = 1f - s;
                score100 -= Mathf.Pow(miss, RAD_GAMMA) * RAD_PENALTY_MAX;
            }
        }

        // --- ATMOSPHERE bonus ---
        {
            float s = ScoreAtmosphereList(
                planetAtmos: planet.conditions.atmosphere,
                creaturePreferred: creature.traits.atmosphere,
                resilience: r
            );
            if (s >= 0f)
                score100 += s * ATMOS_BONUS_MAX;
        }

        // --- LIFESPAN bonus ---
        {
            float s = ScoreLifespanVsYear(
                lifeSpan: creature.traits.lifeSpan,
                lengthOfYear: planet.conditions.lengthOfYear,
                resilience: r
            );
            if (s >= 0f)
                score100 += s * LIFE_BONUS_MAX;
        }

        // --- HABITABLE ZONE bonus ---
        {
            float s = ScoreHabitableZone(
                habitableZone: planet.conditions.habitableZone,
                resilience: r
            );
            if (s >= 0f)
                score100 += s * HZ_BONUS_MAX;
        }

        return Mathf.Clamp(score100, 0f, 100f);
    }

    /// <summary>
    /// Scores biome match. 
    /// - Returns 1 if planetType is in creature's biome list. 
    /// - Otherwise resilience determines how soft the penalty is.
    /// </summary>
    private static float ScoreListContainsToken(string token, IEnumerable<string> allowed, float resilience, float mismatchFloor = 0.05f, float mismatchCeil = 0.6f)
    {
        if (string.IsNullOrWhiteSpace(token)) return -1f;
        if (allowed == null) return -1f;

        // CHANGED: manual case-insensitive compare (no StringComparison)
        string key = token.Trim().ToLowerInvariant();
        if (allowed.Any(a => !string.IsNullOrWhiteSpace(a) && a.Trim().ToLowerInvariant() == key))
            return 1f;

        float floor = Mathf.Clamp01(mismatchFloor);
        float ceil = Mathf.Clamp01(Mathf.Max(mismatchCeil, floor));
        return Mathf.Lerp(floor, ceil, Mathf.Clamp01(resilience));
    }

    /// <summary>
    /// Scores atmosphere coverage.
    /// - Returns 1 if all creaturePreferred gases exist in planetAtmos.
    /// - Otherwise coverage fraction^k, softened by resilience.
    /// </summary>
    private static float ScoreAtmosphereList(string[] planetAtmos, IEnumerable<string> creaturePreferred, float resilience)
    {
        if (planetAtmos == null || creaturePreferred == null) return -1f;

        // CHANGED: build a lowercased set (no StringComparer)
        var planetSet = new HashSet<string>(
            planetAtmos
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim().ToLowerInvariant())
        );

        int needed = 0, present = 0;
        foreach (var pref in creaturePreferred)
        {
            if (string.IsNullOrWhiteSpace(pref)) continue;
            needed++;
            // CHANGED: lowercase before check
            if (planetSet.Contains(pref.Trim().ToLowerInvariant())) present++;
        }

        if (needed == 0) return -1f;
        if (present == needed) return 1f;

        float coverage = (float)present / needed;
        float k = Mathf.Lerp(2.0f, 1.0f, Mathf.Clamp01(resilience));
        return Mathf.Clamp01(Mathf.Pow(coverage, k));
    }

    /// <summary>
    /// Scores temperature compatibility.
    /// - Returns 1 if creature range fully contains planet range.
    /// - Otherwise partial coverage ratio^k, softened by resilience.
    /// </summary>
    private static float ScoreTemperatureRangeContains(float? planetLower, float? planetUpper, float creatureLower, float creatureUpper, float resilience)
    {
        if (planetLower == null || planetUpper == null) return -1f;

        float pLo = planetLower.Value, pHi = planetUpper.Value;
        float cLo = creatureLower, cHi = creatureUpper;
        if (pHi < pLo) (pLo, pHi) = (pHi, pLo);
        if (cHi < cLo) (cLo, cHi) = (cHi, cLo);

        float pLen = Mathf.Max(pHi - pLo, 1e-6f);
        if (cLo <= pLo && cHi >= pHi) return 1f;

        float overlapLower = Mathf.Max(pLo, cLo);
        float overlapUpper = Mathf.Min(pHi, cHi);
        float overlap = Mathf.Max(0f, overlapUpper - overlapLower);
        float coverage = Mathf.Clamp01(overlap / pLen);

        float k = Mathf.Lerp(2.0f, 1.0f, resilience);
        return Mathf.Clamp01(Mathf.Pow(coverage, k));
    }

    /// <summary>
    /// Scores radiation match using inverse-square law.
    /// - Returns 1 if within [radLower, radUpper].
    /// - Otherwise decays smoothly with distance, resilience widens tolerance.
    /// </summary>
    private static float ScoreRadiationFromDistance(float? distanceFromStar, float radLower, float radUpper, float resilience, float referenceDistance)
    {
        if (distanceFromStar == null) return -1f;

        float r = Mathf.Max(distanceFromStar.Value, 0.0001f);
        float R0 = Mathf.Max(referenceDistance, 0.0001f);
        float intensity = (R0 * R0) / (r * r);

        float lo = Mathf.Min(radLower, radUpper);
        float hi = Mathf.Max(radLower, radUpper);
        float baseRange = Mathf.Max(hi - lo, 1e-4f);

        float cushion = baseRange * (0.25f + 0.75f * resilience);
        if (intensity >= lo && intensity <= hi) return 1f;

        float dist = (intensity < lo) ? (lo - intensity) : (intensity - hi);
        float t = dist / Mathf.Max(cushion, 1e-4f);
        return Mathf.Clamp01(1f / (1f + t * t));
    }

    /// <summary>
    /// Scores lifespan vs year length.
    /// - Returns 1 if lifespan >= planet year length.
    /// - Otherwise ratio^k, softened by resilience.
    /// </summary>
    private static float ScoreLifespanVsYear(float lifeSpan, float? lengthOfYear, float resilience)
    {
        if (lengthOfYear == null) return -1f;

        float L = lifeSpan, Y = lengthOfYear.Value;
        if (Y <= 0f || L < 0f) return -1f;
        if (L >= Y) return 1f;

        float ratio = Mathf.Clamp01(L / Y);
        float bend = Mathf.Lerp(3f, 1f, resilience);
        float score = Mathf.Pow(ratio, bend);

        float floor = 0.05f + 0.25f * resilience;
        return Mathf.Clamp01(Mathf.Max(score, floor));
    }

    /// <summary>
    /// Scores habitable zone.
    /// - Returns 1 if in habitable zone, else resilience value.
    /// </summary>
    private static float ScoreHabitableZone(int? habitableZone, float resilience)
    {
        if (habitableZone == null) return -1f;
        if (habitableZone.Value == 1) return 1f;
        if (habitableZone.Value == 0) return resilience;
        return -1f;
    }


    public static List<Creature> updateSanity(List<Creature> creatures)
    {
        // Every 10 seconds every creature loses 0.01
        // Every 10 seconds creatures lose 0.01 based off of their beefs
        // For later -> Users should get a warning when creatures are at 0.1, 0.05, 0.01

        foreach (Creature creature in creatures)
        {
            // Reduce sanity by 0.01
            creature.traits.sanity = Mathf.Max(0f, creature.traits.sanity - 0.01f);

            // Reduce sanity based on beefs
            var currentCreatureNames = creatures.Select(c => c.name).ToHashSet();
            if (creature.traits.beef != null && creature.traits.beef.Count() > 0)
            {
                int validBeefCount = creature.traits.beef.Count(beefName => currentCreatureNames.Contains(beefName));
                float beefPenalty = 0.01f * validBeefCount;
                creature.traits.sanity = Mathf.Max(0f, creature.traits.sanity - beefPenalty);
            }
        }
        return creatures;
    }
}
