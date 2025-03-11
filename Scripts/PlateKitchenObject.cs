using UnityEngine;
using System.Collections.Generic;

public class PlateKitchenObject : KitchenObject
{
    [System.Serializable]
    private struct Ingredient
    {
        public KitchenObjectType type;
        public GameObject visual;
        public GameObject spriteIndicator;
    }

    [SerializeField] private List<Ingredient> validIngredients = new();
    private List<KitchenObjectType> heldKitchenObjects = new();

    public bool TryAddingIngredient(KitchenObject kitchenObject)
    {
        KitchenObjectType ingredientType = kitchenObject.GetKitchenObjectType();

        if (!validIngredients.Exists(x => x.type == ingredientType) || heldKitchenObjects.Contains(ingredientType))
        {
            return false;
        }

        heldKitchenObjects.Add(ingredientType);
        Ingredient ingredient = validIngredients.Find(x => x.type == ingredientType);
        ingredient.visual?.SetActive(true);
        ingredient.spriteIndicator?.SetActive(true);
        return true;
    }

    public bool IsPlateEmpty()
    {
        return heldKitchenObjects.Count == 0;
    }

    public void EmptyPlate()
    {
        heldKitchenObjects.Clear();
        
        foreach (Ingredient ingredient in validIngredients)
        {
            ingredient.visual.SetActive(false);
            ingredient.spriteIndicator.SetActive(false);
        }
    }
}