using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class RatingSystem
{
    public static float GetCreaturePlanetRating(Creature creature, Planet planet)
    {
        if (creature == null || planet == null || planet.conditions == null || creature.traits == null)
            return 0f;

        float r = Mathf.Clamp01(creature.traits.generalResilience);
        int activeTraitsCount = 0;
        float sum = 0f;
        float s;

        // PlanetType
        s = ScoreListContainsToken(
            token: planet.conditions.planetType,
            allowed: creature.traits.biome,
            resilience: r,
            mismatchFloor: 0.05f,
            mismatchCeil: 0.6f
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // Atmosphere
        s = ScoreAtmosphereList(
            planetAtmos: planet.conditions.atmosphere,
            creaturePreferred: creature.traits.atmosphere,
            resilience: r
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // Temperature range
        s = ScoreTemperatureRangeContains(
            planetLower: planet.conditions.tempLower,
            planetUpper: planet.conditions.tempUpper,
            creatureLower: creature.traits.tempLower,
            creatureUpper: creature.traits.tempUpper,
            resilience: r
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // Radiation via distance
        s = ScoreRadiationFromDistance(
            distanceFromStar: planet.conditions.distanceFromStar,
            radLower: creature.traits.radiationLower,
            radUpper: creature.traits.radiationUpper,
            resilience: r,
            referenceDistance: 1.0f
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // Lifespan vs year length
        s = ScoreLifespanVsYear(
            lifeSpan: creature.traits.lifeSpan,
            lengthOfYear: planet.conditions.lengthOfYear,
            resilience: r
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // Habitable zone
        s = ScoreHabitableZone(
            habitableZone: planet.conditions.habitableZone,
            resilience: r
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        if (activeTraitsCount == 0) return 0f;
        return Mathf.Clamp(10f * (sum / activeTraitsCount), 0f, 10f);
    }


    private static float ScoreListContainsToken(string token, IEnumerable<string> allowed, float resilience, float mismatchFloor = 0.05f, float mismatchCeil = 0.6f)
    {
        string t = NormalizeToken(token);
        if (string.IsNullOrEmpty(t)) return -1f;
        if (allowed == null) return -1f; // just in case

        if (allowed.Any(a => NormalizeToken(a) == t)) return 1f;

        float floor = Mathf.Clamp01(mismatchFloor);
        float ceil = Mathf.Clamp01(Mathf.Max(mismatchCeil, floor));
        return Mathf.Lerp(floor, ceil, Mathf.Clamp01(resilience));
    }

    private static float ScoreAtmosphereList(string[] planetAtmos, IEnumerable<string> creaturePreferred, float resilience)
    {
        if (planetAtmos == null || creaturePreferred == null) return -1f;

        var planetSet = planetAtmos
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(NormalizeToken)
            .Where(s => !string.IsNullOrEmpty(s))
            .ToHashSet();

        int needed = 0, present = 0;
        foreach (var pref in creaturePreferred)
        {
            string t = NormalizeToken(pref);
            if (string.IsNullOrEmpty(t)) continue;
            needed++;
            if (planetSet.Contains(t)) present++;
        }

        if (needed == 0) return -1f;
        if (present == needed) return 1f;

        float coverage = (float)present / needed;
        float k = Mathf.Lerp(2.0f, 1.0f, Mathf.Clamp01(resilience));
        return Mathf.Clamp01(Mathf.Pow(coverage, k));
    }

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

        float k = Mathf.Lerp(2.0f, 1.0f, Mathf.Clamp01(resilience));
        return Mathf.Clamp01(Mathf.Pow(coverage, k));
    }

    private static float ScoreRadiationFromDistance(float? distanceFromStar, float radLower, float radUpper, float resilience, float referenceDistance)
    {
        if (distanceFromStar == null) return -1f;

        float r = Mathf.Max(distanceFromStar.Value, 0.0001f);
        float R0 = Mathf.Max(referenceDistance, 0.0001f);
        float intensity = (R0 * R0) / (r * r);

        float lo = Mathf.Min(radLower, radUpper);
        float hi = Mathf.Max(radLower, radUpper);
        float baseRange = Mathf.Max(hi - lo, 1e-4f);

        float cushion = baseRange * (0.25f + 0.75f * Mathf.Clamp01(resilience));
        if (intensity >= lo && intensity <= hi) return 1f;

        float dist = (intensity < lo) ? (lo - intensity) : (intensity - hi);
        float t = dist / Mathf.Max(cushion, 1e-4f);
        return Mathf.Clamp01(1f / (1f + t * t));
    }

    private static float ScoreLifespanVsYear(float lifeSpan, float? lengthOfYear, float resilience)
    {
        if (lengthOfYear == null) return -1f;

        float L = lifeSpan, Y = lengthOfYear.Value;
        if (Y <= 0f || L < 0f) return -1f;
        if (L >= Y) return 1f;

        float ratio = Mathf.Clamp01(L / Y);
        float bend = Mathf.Lerp(3f, 1f, Mathf.Clamp01(resilience));
        float score = Mathf.Pow(ratio, bend);

        float floor = 0.05f + 0.25f * Mathf.Clamp01(resilience);
        return Mathf.Clamp01(Mathf.Max(score, floor));
    }

    private static float ScoreHabitableZone(int? habitableZone, float resilience)
    {
        if (habitableZone == null) return -1f;
        if (habitableZone.Value == 1) return 1f;
        if (habitableZone.Value == 0) return Mathf.Clamp01(resilience);
        return -1f;
    }

    // helper functinos
    private static string NormalizeToken(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return string.Empty;
        s = s.Trim().ToLowerInvariant();
        if (s == "carbondioxide" || s == "carbon_dioxide" || s == "co2") return "carbondioxide";
        if (s == "o2" || s == "oxygen") return "oxygen";
        if (s == "rocky" || s == "rock") return "rock";
        if (s == "water" || s == "ocean" || s == "aquatic") return "ocean";
        return s;
    }


    public static List<Creature> updateSanity(List<Creature> creatures)
    {
        // Every 10 seconds every creature loses 0.01
        // Every 10 seconds creatures lose 0.01 based off of their beefs
        // For later -> Users should get a warning when creatures are at 0.1, 0.05, 0.01

        foreach(Creature creature in creatures)
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
