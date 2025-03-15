using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();

        if (playerObject is PlateKitchenObject)
        {
            Debug.Log("Is valid order: " + DeliveryManager.instance.DeliverRecipe(playerObject as PlateKitchenObject));
        }
    }
}