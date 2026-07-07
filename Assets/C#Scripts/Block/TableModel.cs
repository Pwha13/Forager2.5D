using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableModel : MonoBehaviour
{
    public InventoryHander hander;
    public List<Recipe> recipes;
    public List<Recipe> lastRecipes;
    public List<bool> recipesMakable;
    public RecipeType type;
    public bool isMaking;
    public float currentWaitTime;
    public float makeTime;
    public Vector3 instanceOffset;

    private void Start()
    {
        hander = FindObjectOfType<InventoryHander>();
        GetRecipes();
        UpdateRecipes();
    }

    public void GetRecipes()
    {
        recipes = Json2Recipe.Instance.GetList(type);
    }

    public void UpdateRecipes()
    {
        lastRecipes.Clear();
        lastRecipes.AddRange(recipes.Where(r => r.groupID == 0));
        lastRecipes.AddRange(
            recipes.Where(r => r.toolLevel != 0)
                .GroupBy(r => r.groupID)
                .Select(g => g.OrderBy(r => r.toolLevel).FirstOrDefault(r => !r.isMake))
                .Where(r => r != null)
        );
    }

    public void CheckRecipes()
    {
        recipesMakable.Clear();
        foreach (var recipe in lastRecipes)
        {
            bool check1 = hander.GetDict(recipe.item1ID) >= recipe.item1Count;
            bool check2 = recipe.item2ID == 0 || hander.GetDict(recipe.item2ID) >= recipe.item2Count;
            bool check3 = recipe.item3ID == 0 || hander.GetDict(recipe.item3ID) >= recipe.item3Count;
            if (check1 && check2 && check3) recipesMakable.Add(true);
            else recipesMakable.Add(false);
        }
    }

    public void Make(int i)
    {
        if (isMaking) return;
        Recipe recipe = lastRecipes[i];
        hander.SubDict(recipe.item1ID, recipe.item1Count);
        if (recipe.item2ID != 0) hander.SubDict(recipe.item2ID, recipe.item2Count);
        if (recipe.item3ID != 0) hander.SubDict(recipe.item3ID, recipe.item3Count);
        StartCoroutine(MakeTime(i));
    }

    private IEnumerator MakeTime(int i)
    {
        if (isMaking) yield break;
        isMaking = true;
        makeTime = lastRecipes[i].time;
        while (currentWaitTime < makeTime)
        {
            currentWaitTime += Time.deltaTime;
            yield return null;
        }
        currentWaitTime = 0;
        makeTime = 0;
        isMaking = false;
        GameObject result = ItemDataBaseSO.itemDataBase.GetItem(lastRecipes[i].resultID).itemPrefab;
        Instantiate(result, transform.position + instanceOffset, result.transform.rotation);
        if (lastRecipes[i].groupID != 0)
        {
            var targetRecipe = recipes.FirstOrDefault(r => r.resultID == lastRecipes[i].resultID);
            if (targetRecipe != null)
                targetRecipe.isMake = true;
            UpdateRecipes();
        }
    }

    public void InvokeOnMake(bool b)
    {
        UIEventCenter.Instance.RaiseOnMake(b);
    }

    public void TimeBoost(float intensity)
    {
        currentWaitTime += intensity;
    }
}
