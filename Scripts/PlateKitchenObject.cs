using UnityEngine;
using System.Collections.Generic;

public class PlateKitchenObject : KitchenObject
{
    private static List<KitchenObjectType> validIngredients = new()
    {
        KitchenObjectType.Bread,
        KitchenObjectType.CabbageSliced,
        KitchenObjectType.TomatoSlices,
        KitchenObjectType.CheeseSlices,
        KitchenObjectType.MeatPattyCooked,
    };

    private List<KitchenObjectType> heldKitchenObjects = new();

    public bool TryAddingIngredient(KitchenObjectType kitchenObjectType)
    {
        if (!validIngredients.Contains(kitchenObjectType))
        {
            return false;
        }

        if (heldKitchenObjects.Contains(kitchenObjectType))
        {
            return false;
        }
        else
        {
            heldKitchenObjects.Add(kitchenObjectType);
            return true;
        }
    }

    public bool IsPlateEmpty()
    {
        return heldKitchenObjects.Count == 0;
    }

    public void EmptyPlate()
    {
        heldKitchenObjects.Clear();
    }

    public static bool IsValidIngrdient(KitchenObjectType kitchenObjectType)
    {
        return validIngredients.Contains(kitchenObjectType);
    }
}