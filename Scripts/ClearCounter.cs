using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Component references")]
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;

    private void Start()
    {
        SetMaterial(defaultMaterial);
    }

    public override void HighlightCounter()
    {
        SetMaterial(selectedMaterial);
    }

    public override void UnHighlightCounter()
    {
        SetMaterial(defaultMaterial);
    }

    private void SetMaterial(Material material)
    {
        foreach (MeshRenderer renderer in counterRenderers)
        {
            renderer.material = material;
        }
    }

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