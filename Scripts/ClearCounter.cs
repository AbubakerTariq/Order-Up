using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;

    public override void Interact(Player player)
    {
        KitchenObject counterObject = GetKitchenObject();
        KitchenObject playerObject = player.GetKitchenObject();

        if (counterObject && !playerObject)
        {
            counterObject.SetKitchenObjectParent(player);
        }
        else if (!counterObject && playerObject)
        {
            playerObject.SetKitchenObjectParent(this);
        }
        else if (counterObject && playerObject)
        {
            if (playerObject is PlateKitchenObject plate && plate.TryAddingIngredient(counterObject))
            {
                counterObject.DestroySelf();
            }
            else if (counterObject is PlateKitchenObject counterPlate && counterPlate.TryAddingIngredient(playerObject))
            {
                playerObject.DestroySelf();
            }
        }
    }

    public Transform GetKitchenObjectHoldPoint()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}