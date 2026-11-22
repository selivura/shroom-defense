using UnityEngine.InputSystem;

namespace ShroomDefense.Input
{
    public class UIActionsInput : InputContext, GameInputActions.IUIActions
    {
        public UIActionsInput(GameInputActions actions) : base(actions)
        {
        }

        internal override void Enable()
        {
            actions.UI.Enable();
        }

        internal override void Disable()
        {
            actions.UI.Disable();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}