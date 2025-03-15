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

        playerInputActions.Player.Interact.performed += _ => OnInteract?.Invoke();
        playerInputActions.Player.Operate.performed += _ => OnOperate?.Invoke();
        playerInputActions.Player.StandStill.started += _ => standStillHeld = true;
        playerInputActions.Player.StandStill.canceled += _ => standStillHeld = false;
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