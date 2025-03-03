using UnityEngine;

public class Player : MonoBehaviour
{
    [Space] [Header("Configurable values")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    [Space] [Header("Component references")]
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Animator playerAnimator;

    // String constants
    private const string IsWalking = "IsWalking";

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        transform.position += moveSpeed * Time.deltaTime * moveDir;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);

        playerAnimator.SetBool(IsWalking, moveDir != Vector3.zero);
    }
}