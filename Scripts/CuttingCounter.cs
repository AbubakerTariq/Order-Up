using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CuttingCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Component references")]
    [SerializeField] private Animator counterAnimator;
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    [Space] [Header("UI")]
    [SerializeField] private GameObject ProgressBarUI;
    [SerializeField] private Image progressBar;
    [SerializeField] private Gradient fillGradient;

    private KitchenObject kitchenObject;
    private const string Cut = "Cut";
    private int currentCuts = 0;

    private void Start()
    {
        SetMaterial(defaultMaterial);
        ProgressBarUI.SetActive(false);
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
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            currentCuts = 0;
            progressBar.fillAmount = 0f;
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetKitchenObjectParent(player);
            ProgressBarUI.SetActive(false);
        }
        else if (HasKitchenObject() && player.HasKitchenObject() && player.GetKitchenObject() is PlateKitchenObject)
        {
            PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
            if (plateKitchenObject.TryAddingIngredient(GetKitchenObject()))
            {
                GetKitchenObject().DestroySelf();
            }
        }
    }

    public override void Operate(Player player)
    {
        if (HasKitchenObject() && IsCuttable(GetKitchenObject(), out KitchenObject cutObject, out int cutsNeeded))
        {
            currentCuts++;
            counterAnimator.SetTrigger(Cut);
            UpdateProgressUI((float)currentCuts / cutsNeeded);

            if (currentCuts >= cutsNeeded)
            {
                ProgressBarUI.SetActive(false);
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cutObject, this);
            }
        }
    }

    private bool IsCuttable(KitchenObject kitchenObject, out KitchenObject cutObject, out int numberOfCuts) {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingRecipes) {
            if (kitchenObject.GetKitchenObjectType() == cuttingRecipe.input.GetKitchenObjectType()) {
                numberOfCuts = cuttingRecipe.numberOfCuts;
                cutObject = cuttingRecipe.output;
                return true;
            }
        }
        numberOfCuts = 0;
        cutObject = null;
        return false;
    }

    private void UpdateProgressUI(float progress)
    {   
        if (!ProgressBarUI.activeSelf) ProgressBarUI.SetActive(true);
        progressBar.DOFillAmount(progress, 0.1f).OnUpdate(() => 
        {
            progressBar.color = fillGradient.Evaluate(progressBar.fillAmount);
        });
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