using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class RatingSystem
{
    public static float GetCreaturePlanetRating(Creature creature, Planet planet)
    {
        //proposed: return a value from 1 - 10 based on creature traits, planet conditions matches

        int activeTraitsCount = 0;
        if (creature == null || planet == null || planet.conditions == null || creature.traits == null) return 0f;


        float r = Mathf.Clamp01(creature.traits.generalResilience);
        float sum = 0f;

        // Creature Biome to Planet Type Match
        float s = ScoreListContainsToken(
            token: planet.conditions.planetType,
            allowed: creature.traits.biome,
            resilience: r,
            mismatchFloor: 0.05f,
            mismatchCeil: 0.6f
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }


        // atmosphere match
        s = ScoreAtmosphereList(
            planetAtmos: planet.conditions.atmosphere,
            creaturePreferred: creature.traits.atmosphere,
            resilience: r
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // temperature range match
        s = ScoreTemperatureRangeContains(
            planetLower: planet.conditions.tempLower,
            planetUpper: planet.conditions.tempUpper,
            creatureLower: creature.traits.tempLower,
            creatureUpper: creature.traits.tempUpper,
            resilience: r
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // radiation match
        s = ScoreRadiationFromDistance(
            distanceFromStar: planet.conditions.distanceFromStar,
            radLower: creature.traits.radiationLower,
            radUpper: creature.traits.radiationUpper,
            resilience: r,
            referenceDistance: 1.0f // Assuming a reference distance of 1 AU
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // lifespan vs year match
        s = ScoreLifespanVsYear(
            lifeSpan: creature.traits.lifeSpan,
            lengthOfYear: planet.conditions.lengthOfYear,
            resilience: r
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }

        // habitable zone match
        s = ScoreHabitableZone(
            habitableZone: planet.conditions.habitableZone,
            resilience: r
        );
        if (s >= 0f) { sum += s; activeTraitsCount++; }











        //for now just return 1
        return 1f;
    }

    private static float ScoreListContainsToken(string token, string[] allowed, float resilience, float mismatchFloor = 0.05f, float mismatchCeil = 0.6f)
    {
        if (string.IsNullOrEmpty(token)) return -1f;
        if (allowed == null || allowed.Length == 0) return -1f;

        string t = NormalizeToken(token);
        if (allowed.Contains(t)) return 1f;

        float floor = Mathf.Clamp01(mismatchFloor);
        float ceil = Mathf.Clamp01(Mathf.Max(mismatchCeil, floor));
        return Mathf.Lerp(floor, ceil, Mathf.Clamp01(resilience));
    }

    private static float ScoreAtmosphereList(string[] planetAtmos, string[] creaturePreferred, float resilience)
    {
        if (creaturePreferred == null || creaturePreferred.Length == 0) return -1f;
        if (planetAtmos == null) return -1f;

        var planetSet = planetAtmos.Select(NormalizeToken).ToHashSet();
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

    private static float ScoreTemperatureRangeContains(float planetLower, float planetUpper, float creatureLower, float creatureUpper, float resilience)
    {
        if (float.IsNaN(planetLower) || float.IsNaN(planetUpper)) return -1f;
        if (float.IsNaN(creatureLower) || float.IsNaN(creatureUpper)) return -1f;

        float pLen = Mathf.Max(planetUpper - planetLower, 1e-6f);
        bool contains = (creatureLower <= planetLower) && (creatureUpper >= planetUpper);
        if (contains) return 1f;

        float overlapLower = Mathf.Max(planetLower, creatureLower);
        float overlapUpper = Mathf.Min(planetUpper, creatureUpper);
        float overlap = Mathf.Max(0f, overlapUpper - overlapLower);
        float coverage = Mathf.Clamp01(overlap / pLen);

        float k = Mathf.Lerp(2.0f, 1.0f, Mathf.Clamp01(resilience));
        return Mathf.Clamp01(Mathf.Pow(coverage, k));
    }

    // Radiation proxy from distance (inverse-square), resilience widens tolerance.
    //chat wrote this not me sooooooo if it doesn't work blame chat
    private static float ScoreRadiationFromDistance(float distanceFromStar, float radLower, float radUpper, float resilience, float referenceDistance)
    {
        if (distanceFromStar <= 0f || float.IsNaN(distanceFromStar)) return -1f;

        float R0 = Mathf.Max(referenceDistance, 0.0001f);
        float r = Mathf.Max(distanceFromStar, 0.0001f);
        float intensity = (R0 * R0) / (r * r);

        if (radUpper < radLower) (radLower, radUpper) = (radUpper, radLower);
        float baseRange = Mathf.Max(radUpper - radLower, 1e-4f);

        float cushion = baseRange * (0.25f + 0.75f * Mathf.Clamp01(resilience));
        if (intensity >= radLower && intensity <= radUpper) return 1f;

        float dist = (intensity < radLower) ? (radLower - intensity) : (intensity - radUpper);
        float t = dist / Mathf.Max(cushion, 1e-4f);
        float score = 1f / (1f + t * t);
        return Mathf.Clamp01(score);
    }

    private static float ScoreLifespanVsYear(float lifeSpan, float lengthOfYear, float resilience)
    {
        if (lengthOfYear <= 0f || lifeSpan < 0f) return -1f;
        if (lifeSpan >= lengthOfYear) return 1f;

        float ratio = Mathf.Clamp01(lifeSpan / lengthOfYear);
        if (ratio >= 1f) return 1f;
        float bend = Mathf.Lerp(3f, 1.0f, Mathf.Clamp01(resilience));
        float score = Mathf.Pow(ratio, bend);

        float floor = 0.05f + 0.25f * Mathf.Clamp01(resilience);
        return Mathf.Clamp01(Mathf.Max(score, floor));
    }

    private static float ScoreHabitableZone(int habitableZone, float resilience)
    {
        if (habitableZone == 1) return 1f;
        if (habitableZone == 0) return Mathf.Clamp01(resilience);
        return -1f;
    }








    //helpers

    private static string NormalizeToken(string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        s = s.Trim().ToLowerInvariant();
        if (s == "carbondioxide" || s == "carbon_dioxide" || s == "co2") return "carbondioxide";
        if (s == "o2" || s == "oxygen") return "oxygen";
        if (s == "rocky" || s == "rock") return "rock";
        if (s == "water" || s == "ocean" || s == "aquatic") return "ocean";
        return s;
    }

}
