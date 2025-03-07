using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    public KitchenObjectSO kitchenObjectSO;
    public KitchenObjectSO.KitchenObjectTypes kitchenObjectType;
    private IKitchenObjectParent kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        this.kitchenObjectParent?.ClearKitchenObject(); // Change kitchen object of old parent to null
        this.kitchenObjectParent = kitchenObjectParent; // Set new parent
        transform.parent = kitchenObjectParent.GetKitchenObjectHoldPoint();
        transform.localPosition = Vector3.zero;

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