using UnityEngine;

public static class RatingSystem
{
    public static float GetCreaturePlanetRating(Creature creature, Planet planet)
    {
        //proposed: return a value from 1 - 10 based on creature traits, planet conditions matches

        activeTraitsCount = 0;
        if (creature == null || planet == null || planet.conditions == null || creature.traits == null) return 0f;


        float r = Mathf.Clamp01(creature.traits.generalResilience);
        float sum = 0f;

            float s = ScoreListContainsToken(
                token: MapPlanetTypeToBiomeToken(planet.conditions.planetType),
                allowed: creature.traits.biome,
                resilience: r,
                mismatchFloor: 0.05f,
                mismatchCeil: 0.6f
            );
        if (s >= 0f)
        {
            sum += s;
            activeTraitsCount++;
        }

        //for now just return 1
        return 1f;
    }
}
