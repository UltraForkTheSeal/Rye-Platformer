using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

//使用ScriptableObject 输入信息与场景独立
[CreateAssetMenu(menuName = "Input Reader Asset")]
public class InputReader : ScriptableObject, PlayerInputActions.IGameplayActions
{
    private PlayerInputActions _playerInputActions;
    public event Action<Vector2> Moved;
    public event Action Jumped;
    public event Action DashPerformed;
    public event Action RestartPerformed;
    public event Action QuitPerformed;
    public event Action LoadCheckpointPerformed;
    public event Action SkipLevelPerformed;
    public event Action AnyButtonPressed;


    public Vector2 MoveVector => _playerInputActions.Gameplay.Move.ReadValue<Vector2>();
    public bool isJumpPressed => _playerInputActions.Gameplay.Jump.WasPressedThisFrame();
    public bool isInteractPressed => _playerInputActions.Gameplay.Interact.WasPressedThisFrame();
    public bool rumblePressed => _playerInputActions.Gameplay.Rumble.WasPressedThisFrame();
    public bool IsInteractPerformed { get; private set; }
    public InputDevice CurrentInputDevice { get; private set; }
    
    public void TogglePlayerActions(bool isOn)
    {
        if (isOn)
        {
            _playerInputActions.Enable();
        }
        else
        {
            _playerInputActions.Disable();
        }
    }
    
    private void OnEnable()
    {
        if (_playerInputActions == null)
        {
            _playerInputActions = new PlayerInputActions();
            //将playerInputActions的回调函数设置到该类实现的接口上
            _playerInputActions.Gameplay.SetCallbacks(this);

            InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Moved?.Invoke(context.ReadValue<Vector2>());

        CurrentInputDevice = context.control.device;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Jumped?.Invoke();
        }
        
        CurrentInputDevice = context.control.device;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            IsInteractPerformed = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsInteractPerformed = false;
        }
        
        CurrentInputDevice = context.control.device;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            DashPerformed?.Invoke();
        }
        
        CurrentInputDevice = context.control.device;
    }

    public void OnRumble(InputAction.CallbackContext context)
    {
        //
        
        CurrentInputDevice = context.control.device;
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            RestartPerformed?.Invoke();
        }
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            QuitPerformed?.Invoke();
        }
    }

    public void OnLoadCheckpoint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            LoadCheckpointPerformed?.Invoke();
        }
    }

    public void OnSkipLevel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SkipLevelPerformed?.Invoke();
        }
    }

    private void OnAnyButtonPressed(InputControl inputControl)
    {
        AnyButtonPressed?.Invoke();
    }
    
}
