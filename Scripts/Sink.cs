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
    [SerializeField] private GameObject ProgressBarUI;
    [SerializeField] private Image progressBar;
    [SerializeField] private Gradient fillGradient;

    private float currentWashTime;
    private bool operating;

    private void Start()
    {
        ProgressBarUI.SetActive(false);
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
            ProgressBarUI.SetActive(false);
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
            counterObject.SetKitchenObjectParent(player);
            ProgressBarUI.SetActive(false);
        }
        else if (!counterObject && playerObject.GetKitchenObjectType() == KitchenObject.KitchenObjectType.DirtyPlate)
        {
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