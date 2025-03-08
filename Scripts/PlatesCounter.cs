using UnityEngine;
using System.Collections.Generic;

public class PlatesCounter : BaseCounter
{
    [Space] [Header("Component references")]
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;

    [Space] [Header("Plate")]
    [SerializeField] private KitchenObject plateKitchenObject;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private int maxPlates = 8;
    private List<GameObject> plateVisualList = new();
    private float plateVisualOffset = 0.15f;

    private void Start()
    {
        SetMaterial(defaultMaterial);

        for (int i = 0; i < maxPlates; i++)
        {
            AddPlate();
        }
    }

    public override void HighlightCounter()
    {
        SetMaterial(selectedMaterial);
    }

    public override void UnHighlightCounter()
    {
        SetMaterial(defaultMaterial);
    }

    private void SetMaterial(Material material)
    {
        foreach (MeshRenderer renderer in counterRenderers)
        {
            renderer.material = material;
        }
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectType() == KitchenObject.KitchenObjectType.Plate && plateVisualList.Count < maxPlates)
        {
            player.GetKitchenObject().DestroySelf();
            AddPlate();
        }
        else if (!player.HasKitchenObject() && plateVisualList.Count > 0)
        {
            KitchenObject.SpawnKitchenObject(plateKitchenObject, player);
            RemovePlate();
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