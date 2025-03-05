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

    // String constants
    private const string IsWalking = "IsWalking";

    private void Start()
    {
        gameInput.OnInteract += () => HandleInteraction();
    }

    private void Update()
    {
        HandleMovement();
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
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactRange);
        if (hit.collider != null)
        {
            Debug.Log("hit: " + hit.collider.name);
        }
        else
        {
            Debug.Log("hit nothing");
        }
    }
}