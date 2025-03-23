using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public Action OnInteract;
    public Action OnOperateStart;
    public Action OnOperateEnd;
    private bool standStillHeld;

    private void Awake()
    {
        playerInputActions = new();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += _ => OnInteract?.Invoke();
        playerInputActions.Player.Operate.started += _ => OnOperateStart?.Invoke();
        playerInputActions.Player.Operate.canceled += _ => OnOperateEnd?.Invoke();
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