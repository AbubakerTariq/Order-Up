using UnityEngine;

public class Counter : MonoBehaviour, IKitchenObjectParent
{
    [Space] [Header("Component references")]
    [SerializeField] private MeshRenderer counterRenderer;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("Kitchen Object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private KitchenObject kitchenObject;
    [SerializeField] private KitchenObject[] allKitchenObjects;

    private void Start()
    {
        counterRenderer.material = defaultMaterial;
    }

    public void HighlightCounter()
    {
        counterRenderer.material = selectedMaterial;
    }

    public void UnHighlightCounter()
    {
        counterRenderer.material = defaultMaterial;
    }

    public void Interact(Player player)
    {
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            kitchenObject.SetKitchenObjectParent(player);
            SetKitchenObject(null);
        }
        else if (!HasKitchenObject() && player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            player.SetKitchenObject(null);
        }
        else if (!HasKitchenObject())
        {
            kitchenObject = Instantiate(allKitchenObjects[Random.Range(0, allKitchenObjects.Length)]);
            kitchenObject.SetKitchenObjectParent(this);
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