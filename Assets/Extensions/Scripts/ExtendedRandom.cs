using UnityEngine;
using System.Collections;

public static class ExtendedRandom
{
    /// <summary>
    /// Returns random bool
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public static bool Bool()
    {
        return (Random.value > 0.5f);
    }

    /// <summary>
    /// Return true if random value is bigger then chance
    /// </summary>
    /// <param name="chance">Chance to get random from</param>
    /// <returns></returns>
    public static bool Chance(float chance)
    {
        return (Random.value > chance);
    }
}
