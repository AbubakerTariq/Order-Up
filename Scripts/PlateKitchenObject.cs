using UnityEngine;
using System.Collections.Generic;

public class PlateKitchenObject : KitchenObject
{
    [System.Serializable]
    private struct ValidIngredientsVisualPair
    {
        public KitchenObjectType type;
        public GameObject visual;
    }
    [SerializeField] private List<ValidIngredientsVisualPair> validIngredientsVisualPairs = new();
    private List<KitchenObjectType> heldKitchenObjects = new();

    private void ResetVisuals()
    {
        foreach (ValidIngredientsVisualPair pair in validIngredientsVisualPairs)
        {
            pair.visual.SetActive(false);
        }
    }

    public bool TryAddingIngredient(KitchenObject ingredient)
    {
        KitchenObjectType ingredientType = ingredient.GetKitchenObjectType();

        if (!validIngredientsVisualPairs.Exists(x => x.type == ingredientType))
        {
            Debug.Log("Ingredient not a part of the recipe");
            return false;
        }

        if (heldKitchenObjects.Contains(ingredientType))
        {
            Debug.Log("Ingredient is already a part of the recipe");
            return false;
        }

        heldKitchenObjects.Add(ingredientType);
        ValidIngredientsVisualPair pair = validIngredientsVisualPairs.Find(x => x.type == ingredientType);
        pair.visual.SetActive(true);
        return true;
    }

    public bool IsPlateEmpty()
    {
        return heldKitchenObjects.Count == 0;
    }

    public void EmptyPlate()
    {
        heldKitchenObjects.Clear();
        ResetVisuals();
    }
}