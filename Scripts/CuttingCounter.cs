using UnityEngine;

public class CuttingCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Component references")]
    [SerializeField] private Animator counterAnimator;
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;
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
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
        }
    }

    public override void Operate(Player player)
    {
        if (HasKitchenObject() && IsCuttable(GetKitchenObject(), out KitchenObject cutObject))
        {
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(cutObject, this);
        }
    }

    private bool IsCuttable(KitchenObject kitchenObject, out KitchenObject cutObject)
    {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingRecipes)
        {
            if (kitchenObject.GetKitchenObjectType() == cuttingRecipe.input.GetKitchenObjectType())
            {
                cutObject = cuttingRecipe.output;
                return true;
            }
        }

        cutObject = null;
        return false;
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