using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Component references")]
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("Kitchen Object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;

    private void Start()
    {
        SetRenderer(defaultMaterial);
    }

    public override void HighlightCounter()
    {
        SetRenderer(selectedMaterial);
    }

    public override void UnHighlightCounter()
    {
        SetRenderer(defaultMaterial);
    }

    private void SetRenderer(Material material)
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
            kitchenObject.SetKitchenObjectParent(player); 
            this.SetKitchenObject(null);
        }
        else if (!HasKitchenObject() && player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            player.SetKitchenObject(null);
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