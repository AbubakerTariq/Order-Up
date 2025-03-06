using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public UnityAction OnInteract;
    public UnityAction OnOperate;
    
    private void Awake()
    {
        playerInputActions = new();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += (InputAction.CallbackContext context) => OnInteract?.Invoke();
        playerInputActions.Player.Operate.performed += (InputAction.CallbackContext context) => OnOperate?.Invoke();
    }
    
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }
}