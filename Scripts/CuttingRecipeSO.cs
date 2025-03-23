using UnityEngine;

[CreateAssetMenu]
public class CuttingRecipeSO : ScriptableObject
{
    public int cuttingTime;
    public KitchenObject input;
    public KitchenObject output;
}