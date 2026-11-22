using UnityEngine.InputSystem;

namespace ShroomDefense.Input
{
    public abstract class InputContext
    {
        protected GameInputActions actions;
        public InputContext(GameInputActions actions)
        {
            this.actions = actions;
        }

        internal abstract void Enable();
        internal abstract void Disable();
    }
}