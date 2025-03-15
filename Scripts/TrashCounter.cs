using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();

        if (playerObject is PlateKitchenObject plate)
        {
            plate.Clear();
        }
        else if (playerObject)
        {
            playerObject.DestroySelf();
        }
    }
}