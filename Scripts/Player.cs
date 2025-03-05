using UnityEngine;

public class Player : MonoBehaviour
{
    [Space] [Header("Player settings")]
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float interactRange = 1f;

    [Space] [Header("Component references")]
    [SerializeField] private CharacterController playerController;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Animator playerAnimator;

    // Other variable
    private Counter selectedCounter;

    // String constants
    private const string IsWalking = "IsWalking";

    private void Start()
    {
        gameInput.OnInteract += () => OnInteractKeyPressed();
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void OnInteractKeyPressed()
    {
        selectedCounter?.Interact();
    }

    private void HandleMovement()
    {
        // Getting input here
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        playerController.Move(moveSpeed * Time.deltaTime * moveDir);

        // Smooth rotation
        if (moveDir.sqrMagnitude > 0.001f) transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);

        // Move animation
        playerAnimator.SetBool(IsWalking, moveDir != Vector3.zero);
    }

    private void HandleInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactRange))
        {
            if (hit.transform.TryGetComponent(out Counter counter))
            {
                if (selectedCounter != counter)
                {
                    SetSelectedCounter(counter);
                    selectedCounter.HighlightCounter();
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

    private void SetSelectedCounter(Counter counter)
    {
        selectedCounter?.UnHighlightCounter();
        selectedCounter = counter;
    }
}