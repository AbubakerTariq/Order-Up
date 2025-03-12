using UnityEngine;
using UnityEngine.UI;

public class StoveCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private CookingRecipeSO[] cookingRecipes;

    [Space] [Header("VFX objects")]
    [SerializeField] private GameObject sizzlingParticles;
    [SerializeField] private GameObject stoveOnVisual;

    [Space] [Header("UI")]
    [SerializeField] private GameObject ProgressBarUI;
    [SerializeField] private Image progressBar;
    [SerializeField] private Gradient fillGradient;
    private KitchenObject kitchenObject;
    private float currentCookingTime = 0f;

    private void Start()
    {
        ResetCooking();
    }

    private void Update()
    {
        if (HasKitchenObject() && IsCookable(GetKitchenObject(), out KitchenObject cookedObject, out float maxCookingTime))
        {
            currentCookingTime += Time.deltaTime;
            float progress = currentCookingTime / maxCookingTime;
            UpdateProgressUI(progress);

            if (currentCookingTime / maxCookingTime >= 1f)
            {
                ResetCooking();
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cookedObject, this);
            }
        }
    }

    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();
        KitchenObject counterObject = GetKitchenObject();

        if (!counterObject && playerObject && IsCookable(playerObject))
        {
            playerObject.SetKitchenObjectParent(this);
        }
        else if (counterObject && !playerObject)
        {
            counterObject.SetKitchenObjectParent(player);
            ResetCooking();
        }
        else if (counterObject && playerObject is PlateKitchenObject plate && plate.TryAddingIngredient(counterObject))
        {
            counterObject.DestroySelf();
            ResetCooking();
        }
    }

    private bool IsCookable(KitchenObject kitchenObject)
    {
        foreach (CookingRecipeSO cookingRecipe in cookingRecipes)
        {
            if (kitchenObject.GetKitchenObjectType() == cookingRecipe.input.GetKitchenObjectType())
            {
                return true;
            }
        }
        return false;
    }

    private bool IsCookable(KitchenObject kitchenObject, out KitchenObject cookedObject, out float cookingTime)
    {
        foreach (CookingRecipeSO cookingRecipe in cookingRecipes)
        {
            if (kitchenObject.GetKitchenObjectType() == cookingRecipe.input.GetKitchenObjectType())
            {
                cookingTime = cookingRecipe.cookingTime;
                cookedObject = cookingRecipe.output;
                return true;
            }
        }
        cookingTime = 0f;
        cookedObject = null;
        return false;
    }

    private void UpdateProgressUI(float progress)
    {   
        if (!ProgressBarUI.activeSelf) ProgressBarUI.SetActive(true);
        if (!sizzlingParticles.activeSelf) sizzlingParticles.SetActive(true);
        if (!stoveOnVisual.activeSelf) stoveOnVisual.SetActive(true);

        progressBar.fillAmount = progress;
        progressBar.color = fillGradient.Evaluate(progressBar.fillAmount);
    }

    private void ResetCooking()
    {
        currentCookingTime = 0f;
        progressBar.fillAmount = 0f;
        sizzlingParticles.SetActive(false);
        stoveOnVisual.SetActive(false);
        ProgressBarUI.SetActive(false);
    }

    public Transform GetKitchenObjectHoldPoint()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}