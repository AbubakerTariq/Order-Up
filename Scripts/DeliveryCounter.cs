using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    [Space] [Header("Dirty plate")]
    [SerializeField] private KitchenObject dirtyPlate;
    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();

        if (playerObject is PlateKitchenObject plate && DeliveryManager.instance.DeliverRecipe(plate))
        {
            plate.DestroySelf();
            KitchenObject.SpawnKitchenObject(dirtyPlate, player);
        }
    }
}