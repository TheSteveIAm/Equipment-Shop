using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe", order = 5)]
public class RecipeInfo : ScriptableObject
{
    //List of item requirements this receipe needs to craft an item
    public ItemRequirement[] requirements;
    //Item this recipe produces
    public ItemCode itemCreated;
    //Time it takes to process this recipe (if any)
    public float processTime = 0;
}