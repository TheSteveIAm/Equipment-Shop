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
public class Forge : Station
{
    //if true, currently processing an item and don't accept an other items until finished
    private bool processingItem;

    //particles to play when processing
    public ParticleSystem forgeParticles;

    /// <summary>
    /// Receive an item if not already processing another
    /// And there is an existing recipe for that item
    /// </summary>
    /// <param name="item"></param>
    public override bool GiveItem(Item item)
    {
        if (!processingItem)
        {
            //Find matching recipe, if it exists, start processing item
            for (int i = 0; i < possibleRecipes.Length; i++)
            {
                //Check if item matches requirement for this recipe
                //ASSUMPTION: the forge will only require a single item to smelt, this is likely to change
                //possible change: add a bellows, start the forge when player presses on the bellows instead of automatically
                if (item.itemCode == possibleRecipes[i].requirements[0].item)
                {
                    //accept item, give to recipe, start smelting timer
                    processingItem = true;
                    currentRecipe = Instantiate(possibleRecipes[i], transform); //APPARENTLY THIS WORKS ON THE PREFAB. CREATE AN INSTANCE OF THE RECIPE AND USE THAT PLEASE.
                    currentRecipe.GiveItem(item);
                    return true;
                }
            }
        }
        return false;
    }

    void Update()
    {
        if (currentRecipe != null &&
            currentRecipe.requirementsMet &&
            !currentRecipe.itemReady)
        {
            currentRecipe.ProcessItem();
            if (!forgeParticles.isPlaying) forgeParticles.Play();
            //count timer down until metal bar comes out
            if (currentRecipe.itemReady)
            {
                CreateItem(currentRecipe.CraftItem());
                if (forgeParticles.isPlaying) forgeParticles.Stop();
                Destroy(currentRecipe.gameObject);
                currentRecipe = null;
                processingItem = false;
            }
        }
    }

}
