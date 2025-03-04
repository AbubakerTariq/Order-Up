using UnityEngine;

public class Player : MonoBehaviour
{
    [Space] [Header("Player settings")]
    [SerializeField] private float playerRadius = 0.7f;
    [SerializeField] private float playerHeight = 1.5f;
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float rotateSpeed = 5f;

    [Space] [Header("Component references")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Animator playerAnimator;

    // String constants
    private const string IsWalking = "IsWalking";

    private void Update()
    {
        // Getting input here
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        
        // Movement logic
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Attempt only X Axis movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Attempt only Z Axis movement
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Can't move in any direction
                }
            }
        }
        
        if (canMove) 
        {
            transform.position += moveDistance * moveDir;
        }

        // Smooth rotation
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        
        // Move animation
        playerAnimator.SetBool(IsWalking, moveDir != Vector3.zero);
    }
}