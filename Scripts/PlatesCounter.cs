using UnityEngine;
using System.Collections.Generic;

public class PlatesCounter : BaseCounter
{
    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;

    [Space] [Header("Plate")]
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private int maxPlates = 8;

    [Space] [Header("Audio clips")]
    [SerializeField] private AudioClip pickSound;
    [SerializeField] private AudioClip dropSound;

    private List<GameObject> plateVisualList = new();
    private float plateVisualOffset = 0.15f;

    private void Start()
    {
        for (int i = 0; i < maxPlates; i++)
        {
            AddPlate();
        }
    }

    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();

        if (playerObject is PlateKitchenObject plate && plate.IsEmpty() && plateVisualList.Count < maxPlates)
        {
            SoundManager.PlaySound(audioSource, dropSound);
            playerObject.DestroySelf();
            AddPlate();
        }
        else if (!playerObject && plateVisualList.Count > 0)
        {
            SoundManager.PlaySound(audioSource, pickSound);
            KitchenObject.SpawnKitchenObject(plateKitchenObject, player);
            RemovePlate();
        }
        else if (playerObject && plateVisualList.Count > 0)
        {
            PlateKitchenObject newPlate = Instantiate(plateKitchenObject);
            if (newPlate.TryAddingIngredient(playerObject))
            {
                SoundManager.PlaySound(audioSource, pickSound);
                playerObject.DestroySelf();
                newPlate.SetKitchenObjectParent(player);
                RemovePlate();
            }
            else
            {
                Destroy(newPlate.gameObject);
            }
        }
    }

    private void AddPlate()
    {
        Transform plateVisual = Instantiate(plateVisualPrefab, kitchenObjectHoldPoint);
        plateVisual.localPosition = new Vector3(0f, plateVisualOffset * plateVisualList.Count, 0f);
        plateVisualList.Add(plateVisual.gameObject);
    }

    private void RemovePlate()
    {
        GameObject plateToRemove = plateVisualList[^1]; // Get last element
        plateVisualList.Remove(plateToRemove);
        Destroy(plateToRemove);
    }
}