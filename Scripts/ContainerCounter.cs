using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Animator")]
    [SerializeField] private Animator counterAnimator;
    
    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private KitchenObject kitchenObject;
    private const string OpenClose = "OpenClose";

    public override void Interact(Player player)
    {
        KitchenObject counterObject = GetKitchenObject();
        KitchenObject playerObject = player.GetKitchenObject();

        if (!playerObject)
        {
            counterAnimator.SetTrigger(OpenClose);
            KitchenObject.SpawnKitchenObject(kitchenObject, player);
        }
        else
        {
            if (playerObject.GetKitchenObjectType() == kitchenObject.GetKitchenObjectType())
            {
                playerObject.DestroySelf();
            }
            else if (playerObject is PlateKitchenObject plate && plate.TryAddingIngredient(counterObject))
            {
                counterAnimator.SetTrigger(OpenClose);
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