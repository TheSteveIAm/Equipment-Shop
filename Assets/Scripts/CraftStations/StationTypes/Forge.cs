using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The forge is used to process metal ores into metal bars
/// -It should be able to figure out which recipe the player wants
/// assuming each metal ore recipe only contains 1 item requirement
/// and all requirements are unique
/// - You should not be able to remove molten metal from the forge (while it's processing)
/// </summary>
public class Forge : Station {

    private bool processingItem;

    public override void GiveItem(Item item)
    {
        base.GiveItem(item);

        //Find matching recipe, if it exists, start processing item
        for(int i = 0; i < possibleRecipes.Length; i++)
        {
            //Check if item matches requirement for this recipe
            //ASSUMPTION: the forge will only require a single item to smelt, this is likely to change
            //possible change: add a bellows, start the forge when player presses on the bellows instead of automatically
            if(item.itemType == possibleRecipes[i].requirements[0].item)
            {
                //accept item, give to recipe, start smelting timer
                processingItem = true;
                currentRecipe = possibleRecipes[i];
                currentRecipe.GiveItem(item);
            }
        }
    }

    void Update()
    {
        if (processingItem)
        {

            //count timer down until metal bar comes out

            //when timer is complete, item must be taken out, the closer to the exact finish time the item is taken out, the higher quality the result will be

        }
    }

}
