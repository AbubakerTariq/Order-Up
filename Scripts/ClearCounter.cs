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
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player); 
        }
        else if (!HasKitchenObject() && player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else if (HasKitchenObject() && player.HasKitchenObject() && player.GetKitchenObject() is PlateKitchenObject)
        {
            PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
            if (plateKitchenObject.TryAddingIngredient(GetKitchenObject().GetKitchenObjectType()))
            {
                GetKitchenObject().DestroySelf();
            }
        }
        else if (HasKitchenObject() && GetKitchenObject() is PlateKitchenObject && player.HasKitchenObject())
        {
            PlateKitchenObject plateKitchenObject = GetKitchenObject() as PlateKitchenObject;
            if (plateKitchenObject.TryAddingIngredient(player.GetKitchenObject().GetKitchenObjectType()))
            {
                player.GetKitchenObject().DestroySelf();
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