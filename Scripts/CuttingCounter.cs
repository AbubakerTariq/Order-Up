using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CuttingCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Animator")]
    [SerializeField] private Animator counterAnimator;

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
        ProgressBarUI.SetActive(false);
    }

    public override void Interact(Player player)
    {
        KitchenObject counterObject = GetKitchenObject();
        KitchenObject playerObject = player.GetKitchenObject();

        if (!counterObject && playerObject && IsCuttable(playerObject))
        {
            playerObject.SetKitchenObjectParent(this);
            currentCuts = 0;
            progressBar.fillAmount = 0f;
        }
        else if (counterObject)
        {
            if (!playerObject)
            {
                counterObject.SetKitchenObjectParent(player);
                ProgressBarUI.SetActive(false);
            }
            else if (playerObject is PlateKitchenObject plate && plate.TryAddingIngredient(counterObject))
            {
                counterObject.DestroySelf();
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

    private bool IsCuttable(KitchenObject kitchenObject)
    {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingRecipes)
        {
            if (kitchenObject.GetKitchenObjectType() == cuttingRecipe.input.GetKitchenObjectType())
            {
                return true;
            }
        }

        return false;
    }

    private bool IsCuttable(KitchenObject kitchenObject, out KitchenObject cutObject, out int numberOfCuts) {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingRecipes)
        {
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