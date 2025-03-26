using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    [Space] [Header("Dirty plate")]
    [SerializeField] private KitchenObject dirtyPlate;

    [Space] [Header("Audio clips")]
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip successSound;

    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();

        if (playerObject is PlateKitchenObject plate && DeliveryManager.instance.DeliverRecipe(plate))
        {
            SoundManager.PlaySound(audioSource, successSound);
            plate.DestroySelf();
            KitchenObject.SpawnKitchenObject(dirtyPlate, player);
        }
        else if (playerObject is PlateKitchenObject)
        {
            SoundManager.PlaySound(audioSource, failSound);
        }
    }
}