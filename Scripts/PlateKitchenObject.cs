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

    private void Start()
    {
        ResetVisuals();
    }

    private void ResetVisuals()
    {
        foreach (ValidIngredientsVisualPair pair in validIngredientsVisualPairs)
        {
            pair.visual.SetActive(false);
        }
    }

    public bool TryAddingIngredient(KitchenObjectType kitchenObjectType)
    {
        if (!IsValidIngrdient(kitchenObjectType))
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
            ValidIngredientsVisualPair pair = validIngredientsVisualPairs.Find(x => x.type == kitchenObjectType);
            pair.visual?.SetActive(true);
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
        ResetVisuals();
    }

    public bool IsValidIngrdient(KitchenObjectType kitchenObjectType)
    {
        return validIngredientsVisualPairs.Exists(x => x.type == kitchenObjectType);
    }
}