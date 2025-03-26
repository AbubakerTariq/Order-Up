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
    [SerializeField] private GameObject progressBarUI;
    [SerializeField] private Image progressBar;
    [SerializeField] private Gradient fillGradient;

    [Space] [Header("Audio clips")]
    [SerializeField] private AudioClip pickSound;
    [SerializeField] private AudioClip dropSound;

    [Space] [Header("Sizzle audio")]
    [SerializeField] private AudioSource sizzleAudioSource;
    [SerializeField] private AudioClip sizzleSound;
    
    private KitchenObject kitchenObject;
    private float currentCookingTime;

    private void Start()
    {
        ResetCooking();
    }

    private void Update()
    {
        KitchenObject counterObject = GetKitchenObject();
        if (!counterObject)
            return;

        if (!IsCookable(counterObject, out KitchenObject cookedObject, out float maxCookingTime))
            return;

        currentCookingTime += Time.deltaTime;
        UpdateProgressUI(currentCookingTime / maxCookingTime);

        if (currentCookingTime / maxCookingTime >= 1f)
        {
            ResetCooking();
            counterObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(cookedObject, this);

            if (IsCookable(cookedObject))
            {
                SoundManager.PlayLoopSound(sizzleAudioSource, sizzleSound);
            }
        }
    }

    public override void Interact(Player player)
    {
        KitchenObject playerObject = player.GetKitchenObject();
        KitchenObject counterObject = GetKitchenObject();

        if (!counterObject && playerObject && IsCookable(playerObject))
        {
            SoundManager.PlaySound(audioSource, dropSound);
            SoundManager.PlayLoopSound(sizzleAudioSource, sizzleSound);
            playerObject.SetKitchenObjectParent(this);
        }
        else if (counterObject && !playerObject)
        {
            SoundManager.PlaySound(audioSource, pickSound);
            counterObject.SetKitchenObjectParent(player);
            ResetCooking();
        }
        else if (counterObject && playerObject is PlateKitchenObject plate && plate.TryAddingIngredient(counterObject))
        {
            SoundManager.PlaySound(audioSource, pickSound);
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
        if (!progressBarUI.activeSelf) progressBarUI.SetActive(true);
        if (!sizzlingParticles.activeSelf) sizzlingParticles.SetActive(true);
        if (!stoveOnVisual.activeSelf) stoveOnVisual.SetActive(true);

        progressBar.fillAmount = progress;
        progressBar.color = fillGradient.Evaluate(progressBar.fillAmount);
    }

    private void ResetCooking()
    {
        SoundManager.StopLoopSound(sizzleAudioSource);
        currentCookingTime = 0f;
        progressBar.fillAmount = 0f;
        sizzlingParticles.SetActive(false);
        stoveOnVisual.SetActive(false);
        progressBarUI.SetActive(false);
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