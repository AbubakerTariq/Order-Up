using UnityEngine;
using UnityEngine.UI;

public class StoveCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Component references")]
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private CookingRecipeSO[] cookingRecipes;

    [Space] [Header("UI")]
    [SerializeField] private GameObject ProgressBarUI;
    [SerializeField] private Image progressBar;
    [SerializeField] private Gradient fillGradient;

    private KitchenObject kitchenObject;
    private float currentCookingTime = 0f;

    private void Start()
    {
        SetMaterial(defaultMaterial);
        ProgressBarUI.SetActive(false);
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
                ProgressBarUI.SetActive(false);
                currentCookingTime = 0f;
                progressBar.fillAmount = 0f;
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cookedObject, this);
            }
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
       if (!HasKitchenObject() && player.HasKitchenObject() && IsCookable(player.GetKitchenObject()))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            currentCookingTime = 0f;
            progressBar.fillAmount = 0f;
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            ProgressBarUI.SetActive(false);
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
        progressBar.fillAmount = progress;
        progressBar.color = fillGradient.Evaluate(progressBar.fillAmount);
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