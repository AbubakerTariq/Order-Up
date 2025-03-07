using UnityEngine;
using UnityEngine.Events;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public UnityAction OnInteract;
    public UnityAction OnOperate;
    private bool standStillHeld;

    private void Awake()
    {
        playerInputActions = new();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += (context) => OnInteract?.Invoke();
        playerInputActions.Player.Operate.performed += (Icontext) => OnOperate?.Invoke();
        playerInputActions.Player.StandStill.started += (context) => standStillHeld = true;
        playerInputActions.Player.StandStill.canceled += (context) => standStillHeld = false;
    }
    
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public bool IsStandStillHeld()
    {
        return standStillHeld;
    }
}