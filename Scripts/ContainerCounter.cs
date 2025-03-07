using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Component references")]
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("Kitchen Object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private KitchenObject kitchenObject;
    [SerializeField] private SpriteRenderer kitchenObjectSprite;

    private void Start()
    {
        kitchenObjectSprite.sprite = kitchenObject.GetKitchenObjectSO().objectSprite;
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
        if (!player.HasKitchenObject())
        {
            KitchenObject spawnedKitchenObject = Instantiate(kitchenObject);
            spawnedKitchenObject.SetKitchenObjectParent(player);
        }
        else if (player.HasKitchenObject() && player.GetKitchenObject().kitchenObjectType == kitchenObject.kitchenObjectType)
        {
            Destroy(player.GetKitchenObject().gameObject);
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