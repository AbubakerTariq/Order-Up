using UnityEngine;

[CreateAssetMenu]
public class CuttingRecipeSO : ScriptableObject
{
    public int numberOfCuts;
    public KitchenObject input;
    public KitchenObject output;
}
