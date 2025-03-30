using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [Space] [Header("Animator")]
    [SerializeField] private Animator counterAnimator;
    
    [Space] [Header("Kitchen object")]
    [SerializeField] private KitchenObject kitchenObjectToSpawn;
    
    private const string OpenClose = "OpenClose";

    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();

        if (!playerObject)
        {
            counterAnimator.SetTrigger(OpenClose);
            KitchenObject.SpawnKitchenObject(kitchenObjectToSpawn, player);
        }
        else
        {
            if (playerObject.GetKitchenObjectType() == kitchenObjectToSpawn.GetKitchenObjectType())
            {
                SoundManager.PlaySound(audioSource, dropSound);
                playerObject.DestroySelf();
            }
            else if (playerObject is PlateKitchenObject plate && plate.TryAddingIngredient(kitchenObjectToSpawn))
            {
                counterAnimator.SetTrigger(OpenClose);
            }
        }
    }
}