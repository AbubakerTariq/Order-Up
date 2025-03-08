using UnityEngine;

[CreateAssetMenu]
public class CookingRecipeSO : ScriptableObject
{
    public float cookingTime;
    public KitchenObject input;
    public KitchenObject output;
}