using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();

        if (playerObject is PlateKitchenObject plate && DeliveryManager.instance.DeliverRecipe(plate))
        {
            plate.Clear();
        }
    }
}