using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerUI : MonoBehaviour
{
    [Space]
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private RectTransform ordersContainer;
    [SerializeField] private OrderUI orderTemplateUI;
    private List<OrderUI> currentOrdersListUI = new();

    private void OnEnable()
    {
        deliveryManager.OnOrderReceived += AddOrder;
        deliveryManager.OnOrderDelivered += RemoveOrder;
    }

    private void OnDisable()
    {
        deliveryManager.OnOrderReceived -= AddOrder;
        deliveryManager.OnOrderDelivered -= RemoveOrder;
    }
    
    private void AddOrder(DeliverableRecipeSO recipe)
    {
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
        if (orderIndex >= 0 && orderIndex < currentOrdersListUI.Count && currentOrdersListUI[orderIndex] != null)
        {
            GameObject objToRemove = currentOrdersListUI[orderIndex].gameObject;
            currentOrdersListUI.RemoveAt(orderIndex);
            Destroy(objToRemove);
        }
    }
}