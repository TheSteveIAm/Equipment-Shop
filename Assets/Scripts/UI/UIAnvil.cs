using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnvil : UIBase
{
    RecipeInfo[] recipes;

    public delegate void RecipeSelectedDelegate(RecipeInfo recipe);
    public static event RecipeSelectedDelegate OnRecipeSelected;

    void OnEnable()
    {
        Anvil.OnAnvilStart += Init;
    }

    void OnDisable()
    {
        Anvil.OnAnvilStart -= Init;
    }

    void Init(RecipeInfo[] recipeList)
    {
        //recipes = new RecipeInfo[recipeList.Length];

        recipes = recipeList;
    }

    public void SelectRecipe(RecipeInfo selected)
    {
        if(selected == null)
        {
            //cancel recipe, return any items in crafting station
        }
        else
        {
            if(OnRecipeSelected != null)
            {
                OnRecipeSelected(selected);
            }
        }
    }
}
