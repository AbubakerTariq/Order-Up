using UnityEngine;

public class TrashCounter : BaseCounter
{
    [Space] [Header("Audio clips")]
    [SerializeField] private AudioClip trashSound;

    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();

        if (playerObject is PlateKitchenObject plate && !plate.IsEmpty())
        {
            SoundManager.PlaySound(audioSource, trashSound);
            plate.Clear();
        }
        else if (playerObject && playerObject is not PlateKitchenObject)
        {
            SoundManager.PlaySound(audioSource, trashSound);
            playerObject.DestroySelf();
        }
    }
}