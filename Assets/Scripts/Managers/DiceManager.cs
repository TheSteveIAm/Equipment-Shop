using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiceManager {

    /// <summary>
    /// returns a dice roll between given range
    /// </summary>
    /// <param name="min">Minimum result (inclusive)</param>
    /// <param name="max">Minimum result (inclusive)</param>
    /// <returns></returns>
    public static int RollDice(int min, int max)
    {
        return Random.Range(min, max + 1);
    }
}
