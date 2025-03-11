using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    public enum KitchenObjectType
    {
        Bread, Tomato, TomatoSlices, Cabbage, CabbageSliced, CheeseBlock, CheeseSlices, MeatPattyBurned, MeatPattyCooked, MeatPattyUncooked, Plate
    }

    [Space] [Header("Kitchen object parameters")]
    [SerializeField] private Sprite kitchenObjectSprite;
    [SerializeField] private KitchenObjectType kitchenObjectType;
    private IKitchenObjectParent kitchenObjectParent;

    public Sprite GetKitchenObjectSprite()
    {
        return kitchenObjectSprite;
    }

    public KitchenObjectType GetKitchenObjectType()
    {
        return kitchenObjectType;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        this.kitchenObjectParent?.ClearKitchenObject(); // Change kitchen object of old parent to null
        this.kitchenObjectParent = kitchenObjectParent; // Set new parent
        transform.parent = kitchenObjectParent.GetKitchenObjectHoldPoint();
        transform.localPosition = Vector3.zero;
        // transform.localRotation = Quaternion.identity;

        kitchenObjectParent.SetKitchenObject(this);
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static void SpawnKitchenObject(KitchenObject kitchenObject, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenObject spawnedKitchenObject = Instantiate(kitchenObject);
        spawnedKitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }
}