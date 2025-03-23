using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    [Space] [Header("Player settings")]
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float interactRange = 1f;

    [Space] [Header("Component references")]
    [SerializeField] private CharacterController playerController;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Animator playerAnimator;

    [Space] [Header("Kitchen object related")]
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private KitchenObject kitchenObject;
    private const string IsWalking = "IsWalking";
    private BaseCounter selectedCounter;

    private void OnEnable()
    {
        gameInput.OnInteract += () => selectedCounter?.Interact(this);
        gameInput.OnOperate += () => selectedCounter?.Operate(this);
        gameInput.OnOperateStart += () => selectedCounter?.OperateStart(this);
        gameInput.OnOperateEnd += () => selectedCounter?.OperateEnd(this);
    }

    private void OnDisable()
    {
        gameInput.OnInteract -= () => selectedCounter?.Interact(this);
        gameInput.OnOperate -= () => selectedCounter?.Operate(this);
        gameInput.OnOperateStart -= () => selectedCounter?.OperateStart(this);
        gameInput.OnOperateEnd -= () => selectedCounter?.OperateEnd(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleMovement()
    {
        // Getting input here
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        if (!gameInput.IsStandStillHeld()) playerController.Move(moveSpeed * Time.deltaTime * moveDir);

        // Smooth rotation
        if (moveDir.sqrMagnitude > 0.001f) transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);

        // Move animation
        playerAnimator.SetBool(IsWalking, moveDir != Vector3.zero);
    }

    private void HandleInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactRange))
        {
            if (hit.transform.TryGetComponent(out BaseCounter counter))
            {
                if (selectedCounter != counter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter counter)
    {
        selectedCounter?.OperateEnd(this);
        selectedCounter?.UnHighlightCounter();
        selectedCounter = counter;
        selectedCounter?.HighlightCounter();
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