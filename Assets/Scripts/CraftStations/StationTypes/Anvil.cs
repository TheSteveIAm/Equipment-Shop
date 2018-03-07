using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : Station
{

    //particles to play when processing
    public ParticleSystem sparkParticles;

    public delegate void AnvilItemCreatedDelegate(Item item);
    public static event AnvilItemCreatedDelegate OnItemCreated;

    public delegate void AnvilStartDelegate(RecipeInfo[] recipeList);
    public static event AnvilStartDelegate OnAnvilStart;

    protected override void Start()
    {
        base.Start();

        if(OnAnvilStart != null)
        {
            OnAnvilStart(possibleRecipes);
        }
    }

    /// <summary>
    /// Receive an item if not already processing another
    /// And there is an existing recipe for that item
    /// </summary>
    /// <param name="item"></param>
    public override bool GiveItem(Item item)
    {
        if (currentRecipe != null)
        {
            for (int i = 0; i < currentRecipe.requirements.Length; i++)
            {
                if (item.itemCode == currentRecipe.requirements[i].item && !currentRecipe.requirements[i].filled)
                {
                    currentRecipe.GiveItem(item);
                    return true;
                }
            }

        }
        return false;
    }

    public void SelectRecipe(RecipeInfo recipeInfo)
    {
        currentRecipe = Instantiate(itemList.recipePrefab, transform); //APPARENTLY THIS WORKS ON THE PREFAB. CREATE AN INSTANCE OF THE RECIPE AND USE THAT PLEASE.
        currentRecipe.requirements = new ItemRequirement[recipeInfo.requirements.Length];
        System.Array.Copy(recipeInfo.requirements, currentRecipe.requirements, recipeInfo.requirements.Length);
        currentRecipe.itemCreated = recipeInfo.itemCreated;
        currentRecipe.processTime = recipeInfo.processTime;
    }

    public override Item Interact()
    {
        //if(currentRecipe.item)

        if (currentRecipe.requirementsMet)
        {
            currentRecipe.ProcessItem(1f);
            sparkParticles.Play();
        }

        if (currentRecipe.itemReady)
        {
            Item item = CreateItem(currentRecipe.itemCreated);

            if(OnItemCreated != null)
            {
                OnItemCreated(item);
            }

            Destroy(currentRecipe.gameObject);
            currentRecipe = null;
        }

        return null;
    }
}
