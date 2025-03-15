using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{
    [Space] [Header("Deliverable recipes")]
    [SerializeField] private List<DeliverableRecipeSO> deliverableRecipes;

    [Space] [Header("Configurations")]
    [SerializeField] private float orderWaitTimeMin = 5f;
    [SerializeField] private float orderWaitTimeMax = 10f;
    [SerializeField] private int maxOrders = 5;

    public static DeliveryManager instance;
    public UnityAction<DeliverableRecipeSO> OnOrderReceived;
    public UnityAction<int> OnOrderDelivered;
    private List<DeliverableRecipeSO> currentOrdersList = new();
    private float currentOrderWaitTime = 0f;
    private float currentOrderWaitTimeElapsed = 0f;

    private void Start()
    {
        instance = this;
        GenerateOrderWaitTime();
    }

    private void Update()
    {
        if (currentOrdersList.Count < maxOrders)
        {
            currentOrderWaitTimeElapsed += Time.deltaTime;
        }

        if (currentOrderWaitTimeElapsed > currentOrderWaitTime)
        {
            DeliverableRecipeSO recipe = deliverableRecipes[Random.Range(0, deliverableRecipes.Count)];
            OnOrderReceived?.Invoke(recipe);
            currentOrdersList.Add(recipe);
            GenerateOrderWaitTime();
        }
    }

    private void GenerateOrderWaitTime()
    {
        currentOrderWaitTimeElapsed = 0f;
        currentOrderWaitTime = Random.Range(orderWaitTimeMin, orderWaitTimeMax);
    }

    public bool DeliverRecipe(PlateKitchenObject plate)
    {
        if (plate.IsEmpty() || currentOrdersList.Count == 0) return false;

        List<KitchenObject.KitchenObjectType> plateIngredients = plate.GetPlateIngredients();
        plateIngredients.Sort();

        for (int i = 0; i < currentOrdersList.Count; i++)
        {
            DeliverableRecipeSO recipe = currentOrdersList[i];

            if (recipe.recipeList.Count != plateIngredients.Count) continue;

            List<KitchenObject.KitchenObjectType> recipeIngredients = new();
            foreach (KitchenObject ingredient in recipe.recipeList)
            {
                recipeIngredients.Add(ingredient.GetKitchenObjectType());
            }
            recipeIngredients.Sort();

            bool isMatch = true;
            for (int j = 0; j < plateIngredients.Count; j++)
            {
                if (plateIngredients[j] != recipeIngredients[j])
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                currentOrdersList.RemoveAt(i);
                OnOrderDelivered?.Invoke(i);
                return true;
            }
        }
        return false;
    }
}