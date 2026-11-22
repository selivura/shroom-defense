using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShroomDefense.Input
{
    public class PlayerActionsInput : InputContext, GameInputActions.IPlayerActions
    {
        public event Action OnPointerClickEvent;
        public event Action OnPointerPressedEvent;
        public event Action OnPointerReleaseEvent;
        public Vector2 PointerPosition { get; private set; }
        public PlayerActionsInput(GameInputActions actions) : base(actions)
        {
        }
        internal override void Enable()
        {
            actions.Player.Enable();
        }

        internal override void Disable()
        {
            actions.Player.Disable();
        }

        public void OnPointerClick(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                OnPointerPressedEvent?.Invoke();
            }
            if(context.canceled)
            {
                OnPointerReleaseEvent?.Invoke();
            }
            if(context.performed)
            {
                OnPointerClickEvent?.Invoke();
            }
        }

        public void OnPointerPosition(InputAction.CallbackContext context)
        {
            PointerPosition = context.ReadValue<Vector2>();
        }
    }
}