using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all characters
/// </summary>
public class Character : MonoBehaviour
{
    /// <summary>
    /// Each character has stats
    /// </summary>
    public Stats stats;

    protected virtual void Start()
    {
        stats = GetComponent<Stats>();
    }
}
