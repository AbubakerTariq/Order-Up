using UnityEngine;
using UnityEngine.UI;

public class CuttingCounter : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Animator")]
    [SerializeField] private Animator counterAnimator;

    [Space] [Header("Kitchen object")]
    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    [Space] [Header("UI")]
    [SerializeField] private GameObject progressBarUI;
    [SerializeField] private Image progressBar;
    [SerializeField] private Gradient fillGradient;

    [Space] [Header("Additional SFX")]
    [SerializeField] private AudioClip cutSound;
    [SerializeField] private float cutSoundInterval = 0.2f;
    
    private KitchenObject kitchenObject;
    private const string Cut = "Cut";
    private float currentCuttingTime;
    private float cutSoundTimer;
    
    private bool operating;

    private void Start()
    {
        progressBarUI.SetActive(false);
    }

    private void Update()
    {
        if (!operating)
            return;

        KitchenObject counterObject = GetKitchenObject();
        if (!counterObject)
            return;

        if (!IsCuttable(counterObject, out KitchenObject cutObject, out float maxCuttingTime))
            return;

        cutSoundTimer -= Time.deltaTime;
        if (cutSoundTimer <= 0f)
        {
            cutSoundTimer = cutSoundInterval;
            SoundManager.PlaySound(audioSource, cutSound);
        }

        currentCuttingTime += Time.deltaTime;
        UpdateProgressUI(currentCuttingTime / maxCuttingTime);

        if (currentCuttingTime / maxCuttingTime >= 1f)
        {
            counterAnimator.SetBool(Cut, false);
            progressBarUI.SetActive(false);
            counterObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(cutObject, this);
        }
    }

    public override void Interact(Player player)
    {
        KitchenObject counterObject = GetKitchenObject();
        KitchenObject playerObject = player.GetKitchenObject();

        if (!counterObject && playerObject && IsCuttable(playerObject))
        {
            playerObject.SetKitchenObjectParent(this);
            currentCuttingTime = 0f;
        }
        else if (counterObject)
        {
            if (!playerObject)
            {
                counterObject.SetKitchenObjectParent(player);
                progressBarUI.SetActive(false);
            }
            else if (playerObject is PlateKitchenObject plate && plate.TryAddingIngredient(counterObject))
            {
                counterObject.DestroySelf();
            }
        }
    }

    public override void OperateStart(Player player)
    {
        operating = true;
        
        KitchenObject counterObject = GetKitchenObject();

        if (counterObject && IsCuttable(counterObject))
        {
            counterAnimator.SetBool(Cut, true);
        }
    }

    public override void OperateEnd(Player player)
    {
        operating = false;
        counterAnimator.SetBool(Cut, false);
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

    private bool IsCuttable(KitchenObject kitchenObject, out KitchenObject cutObject, out float cuttingTime) {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingRecipes)
        {
            if (kitchenObject.GetKitchenObjectType() == cuttingRecipe.input.GetKitchenObjectType()) {
                cuttingTime = cuttingRecipe.cuttingTime;
                cutObject = cuttingRecipe.output;
                return true;
            }
        }
        cuttingTime = 0f;
        cutObject = null;
        return false;
    }

    private void UpdateProgressUI(float progress)
    {   
        if (!progressBarUI.activeSelf) progressBarUI.SetActive(true);

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

        if (this.kitchenObject != null)
        {
            SoundManager.PlaySound(audioSource, dropSound);
        }
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