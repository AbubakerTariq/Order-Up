using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    public enum KitchenObjectTypes
    {
        Bread, Tomato, TomatoSlices, Cabbage, CabbageSliced, CheeseBlock, CheeseSlices, MeatPattyBurned, MeatPattyCooked, MeatPattyUncooked, Plate
    }

    public Transform objectPrefab;
    public Sprite objectSprite;
    public KitchenObjectTypes objectType;
}