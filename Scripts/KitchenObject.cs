using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    public KitchenObjectSO kitchenObjectSO;
    public KitchenObjectSO.KitchenObjectTypes kitchenObjectType;
    private IKitchenObjectParent kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.SetKitchenObject(null);
        }

        this.kitchenObjectParent = kitchenObjectParent;
        transform.parent = kitchenObjectParent.GetKitchenObjectHoldPoint();
        transform.localPosition = Vector3.zero;

        kitchenObjectParent.SetKitchenObject(this);
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
}
