using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Json2Recipe : MonoBehaviour
{
    public static Json2Recipe Instance;
    
    public RecipeList buildRecipes;
    public RecipeList furnaceRecipes;
    public RecipeList forgeRecipes;
    public RecipeList sewRecipes;
    public RecipeList factoryRecipes;

    private void Awake()
    {
        Instance = this;
        GetRecipes();
        GetFurnaceRecipes();
        GetForgeRecipes();
        GetSewRecipes();
    }

    public List<Recipe> GetList(RecipeType type)
    {
        return type switch
        {
            RecipeType.Furnace => furnaceRecipes.recipes,
            RecipeType.Forge => forgeRecipes.recipes,
            RecipeType.SewStation => sewRecipes.recipes,
            RecipeType.Factory => factoryRecipes.recipes,
            _ => null
        };
    }
    public void GetRecipes()
    {
        TextAsset buildRecipeJson = Resources.Load<TextAsset>("JSON/recipes");
        if (buildRecipeJson == null)
        {
            Debug.Log("recipeJson is null");
            return;
        }
        string buildJsonFixed = "{\"recipes\":" + buildRecipeJson.text + "}";
        buildRecipes = JsonUtility.FromJson<RecipeList>(buildJsonFixed);
    }

    public void GetFurnaceRecipes()
    {
        TextAsset furnaceRecipeJson = Resources.Load<TextAsset>("JSON/furnaceRecipes");
        if (furnaceRecipeJson == null)
        {
            Debug.Log("recipeJson is null");
            return;
        }
        string furnaceJsonFixed = "{\"recipes\":" + furnaceRecipeJson.text + "}";
        furnaceRecipes = JsonUtility.FromJson<RecipeList>(furnaceJsonFixed);
    }

    public void GetForgeRecipes()
    {
        TextAsset forgeRecipeJson = Resources.Load<TextAsset>("JSON/forgeRecipes");
        if (forgeRecipeJson == null)
        {
            Debug.Log("recipeJson is null");
            return;
        }
        string forgeJsonFixed = "{\"recipes\":" + forgeRecipeJson.text + "}";
        forgeRecipes = JsonUtility.FromJson<RecipeList>(forgeJsonFixed);
    }
    public void GetSewRecipes()
    {
        TextAsset sewRecipeJson = Resources.Load<TextAsset>("JSON/sewRecipes");
        if (sewRecipeJson == null)
        {
            Debug.Log("recipeJson is null");
            return;
        }
        string sewJsonFixed = "{\"recipes\":" + sewRecipeJson.text + "}";
        sewRecipes = JsonUtility.FromJson<RecipeList>(sewJsonFixed);
    }
}
[Serializable]
public class Recipe
{
    public int recipeID;
    public int resultID;
    public int resultCount;
    public int item1ID;
    public int item1Count;
    public int item2ID;
    public int item2Count;
    public int item3ID;
    public int item3Count;
    public float time;
    public int groupID;
    public int toolLevel;
    public bool isMake;

}
[Serializable]
public class RecipeList
{
    public List<Recipe> recipes;
}

public enum RecipeType
{
    Furnace,Forge,SewStation,Factory,Build
}
