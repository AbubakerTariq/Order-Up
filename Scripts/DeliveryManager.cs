using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeliveryManager : MonoBehaviour
{
    [Space] [Header("Deliverable recipes")]
    [SerializeField] private List<DeliverableRecipeSO> deliverableRecipes;

    [Space] [Header("Configurations")]
    [SerializeField] private float orderWaitTimeMin = 5f;
    [SerializeField] private float orderWaitTimeMax = 10f;
    [SerializeField] private float timePerOrder = 25f;
    [SerializeField] private int maxOrders = 5;

    [Space] [Header("UI")]
    [SerializeField] private RectTransform ordersContainer;
    [SerializeField] private OrderUI orderTemplateUI;

    public static DeliveryManager instance;
    private List<DeliverableRecipeSO> currentOrdersList = new();
    private List<OrderUI> currentOrdersListUI = new();
    private float nextOrderWaitTime = 0f;
    private float nextOrderWaitTimeElapsed = 0f;

    private void Start()
    {
        instance = this;
        GenerateNextOrderWaitTime();
    }

    private void Update()
    {
        if (currentOrdersList.Count < maxOrders)
        {
            nextOrderWaitTimeElapsed += Time.deltaTime;
        }

        if (nextOrderWaitTimeElapsed > nextOrderWaitTime)
        {
            AddOrder();
            GenerateNextOrderWaitTime();
        }
    }

    private void GenerateNextOrderWaitTime()
    {
        nextOrderWaitTimeElapsed = 0f;
        nextOrderWaitTime = Random.Range(orderWaitTimeMin, orderWaitTimeMax);
    }

    private void AddOrder()
    {
        DeliverableRecipeSO recipe = deliverableRecipes[Random.Range(0, deliverableRecipes.Count)];
        currentOrdersList.Add(recipe);

        OrderUI orderUI = Instantiate(orderTemplateUI, ordersContainer);
        orderUI.gameObject.SetActive(true);
        orderUI.recipeNameText.text = recipe.recipeName;
        
        foreach (KitchenObject ingredient in recipe.recipeList)
        {
            Image ingredientImage = Instantiate(orderUI.ingredientImage, orderUI.ingredientsContainer);
            ingredientImage.sprite = ingredient.GetKitchenObjectSprite();
            ingredientImage.gameObject.SetActive(true);
        }

        currentOrdersListUI.Add(orderUI);
    }

    private void RemoveOrder(int orderIndex)
    {
        currentOrdersList.RemoveAt(orderIndex);

        GameObject objToRemove = currentOrdersListUI[orderIndex].gameObject;
        currentOrdersListUI.RemoveAt(orderIndex);
        Destroy(objToRemove);
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
                RemoveOrder(i);
                return true;
            }
        }
        return false;
    }
}