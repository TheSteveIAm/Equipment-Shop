using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Trade class handles trades between the player and Heroes
/// </summary>
public class Trade : MonoBehaviour {

    /// <summary>
    /// Reference to the trading characters
    /// </summary>
    public Player player;
    public Hero hero;

    /// <summary>
    /// Offered gold from each side of the trade
    /// </summary>
    private int playerGold, heroGold;

    /// <summary>
    /// Offered items from each side of the trade
    /// </summary>
    private Item[] playerOwnedItems, heroOwnedItems;
	
    // Use this for initialization
	void Start () {
		
	}


	
}
