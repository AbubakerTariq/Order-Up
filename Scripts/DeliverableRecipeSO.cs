using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class DeliverableRecipeSO : ScriptableObject
{
    public string recipeName;
    public List<KitchenObject> recipeList;
}