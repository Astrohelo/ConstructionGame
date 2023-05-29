using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    private PlayerInputActions playerInputActions;
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnJumpAction;
    public event EventHandler OnPlaceSimpleAction;
    public event EventHandler OnPlaceForeverAction;
    public event EventHandler RotateYAction;
    public event EventHandler RotateZAction;

    public enum Binding {
        Move_Forward,
        Move_Down,
        Move_Left,
        Move_Right,
        Jump,
        Interact,
        Interact2,
        RotateZ,
        RotateY
    }

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.PlaceSimple.performed += PlaceSimple_performed;
        playerInputActions.Player.PlaceForever.performed += PlaceForever_performed;
        playerInputActions.Player.RotateY.performed += RotateY_performed;
        playerInputActions.Player.RotateZ.performed += RotateZ_performed;
    }

    private void RotateY_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        RotateYAction?.Invoke(this, EventArgs.Empty);
    }
    private void RotateZ_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        RotateZAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.Jump.performed -= Jump_performed;
        playerInputActions.Player.PlaceSimple.performed -= PlaceSimple_performed;
        playerInputActions.Player.PlaceForever.performed -= PlaceForever_performed;
        playerInputActions.Player.RotateY.performed -= RotateY_performed;
        playerInputActions.Player.RotateZ.performed -= RotateZ_performed;
        playerInputActions.Dispose();

    }

    private void PlaceForever_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlaceForeverAction?.Invoke(this, EventArgs.Empty);
    }

    private void PlaceSimple_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPlaceSimpleAction?.Invoke(this, EventArgs.Empty);
    }


    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact2:
                return playerInputActions.Player.PlaceForever.bindings[0].ToDisplayString();
            case Binding.Jump:
                return playerInputActions.Player.Jump.bindings[0].ToDisplayString();
            case Binding.RotateY:
                return playerInputActions.Player.RotateY.bindings[0].ToDisplayString();
            case Binding.RotateZ:
                return playerInputActions.Player.RotateZ.bindings[0].ToDisplayString();
            case Binding.Move_Forward:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();

        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInputActions.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
        switch (binding) {
            default:
            case Binding.Move_Forward:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Jump:
                inputAction = playerInputActions.Player.Jump;
                bindingIndex = 0;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact2:
                inputAction = playerInputActions.Player.PlaceForever;
                bindingIndex = 0;
                break;
            case Binding.RotateY:
                inputAction = playerInputActions.Player.RotateY;
                bindingIndex = 0;
                break;
            case Binding.RotateZ:
                inputAction = playerInputActions.Player.RotateZ;
                bindingIndex = 0;
                break;

        }
        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();
        }).Start();
    }
    
}
