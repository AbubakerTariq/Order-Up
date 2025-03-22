using UnityEngine;
using UnityEngine.UI;

public class Sink : BaseCounter, IKitchenObjectParent
{
    [Space] [Header("Kitchen object")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private PlateKitchenObject plate;
    private KitchenObject kitchenObject;

    [Space] [Header("UI")]
    [SerializeField] private GameObject ProgressBarUI;
    [SerializeField] private Image progressBar;
    [SerializeField] private Gradient fillGradient;

    private void Start()
    {
        ProgressBarUI.SetActive(false);
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
        }
    }

    public override void OperateStart(Player player)
    {
        
    }

    public override void OperateEnd(Player player)
    {
        
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