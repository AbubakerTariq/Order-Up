using UnityEngine;
using UnityEngine.UI;

public class Sink : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private PlateKitchenObject plate;
    private KitchenObject kitchenObject;

    [Space] [Header("Configs")]
    [SerializeField] private float washTime = 3.5f;

    [Space] [Header("UI")]
    [SerializeField] private GameObject progressBarUI;
    [SerializeField] private Image progressBar;
    [SerializeField] private Gradient fillGradient;

    [Space] [Header("Audio clips")]
    [SerializeField] private AudioClip pickSound;
    [SerializeField] private AudioClip dropSound;

    private float currentWashTime;
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

        if (counterObject.GetKitchenObjectType() != KitchenObject.KitchenObjectType.DirtyPlate)
            return;
        
        currentWashTime += Time.deltaTime;
        UpdateProgressUI(currentWashTime / washTime);

        if (currentWashTime / washTime >= 1f)
        {
            progressBarUI.SetActive(false);
            counterObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(plate, this);
        }
    }

    public override void Interact(Player player)
    {
        KitchenObject counterObject = GetKitchenObject();
        KitchenObject playerObject = player.GetKitchenObject();

        if (counterObject && !playerObject)
        {
            SoundManager.PlaySound(audioSource, pickSound);
            counterObject.SetKitchenObjectParent(player);
            progressBarUI.SetActive(false);
        }
        else if (!counterObject && playerObject && playerObject.GetKitchenObjectType() == KitchenObject.KitchenObjectType.DirtyPlate)
        {
            SoundManager.PlaySound(audioSource, dropSound);
            playerObject.SetKitchenObjectParent(this);
            currentWashTime = 0f;
        }
    }

    public override void OperateStart(Player player)
    {
        operating = true;
    }

    public override void OperateEnd(Player player)
    {
        operating = false;
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