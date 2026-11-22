using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Input
{
    public class InputService
    {
        public readonly PlayerActionsInput PlayerActionsInput;
        public readonly UIActionsInput UIActionsInput;
        private readonly Stack<InputContext> _contextStack = new();

        public InputService()
        {
            var actions = new GameInputActions();
            actions.Enable();
            PlayerActionsInput = new PlayerActionsInput(actions);
            UIActionsInput = new UIActionsInput(actions);
            PlayerActionsInput.Enable();
        }

        public void PushContext(InputContext context)
        {
            if (_contextStack.Count > 0)
            {
                _contextStack.Peek().Disable();
            }
            else
            {
                PlayerActionsInput.Disable();
            }

            _contextStack.Push(context);
            context.Enable();
        }

        public InputContext PopContext()
        {
            if (_contextStack.Count <= 0)
            {
                return null;
            }

            var context = _contextStack.Pop();
            context.Disable();

            if (_contextStack.Count > 0)
            {
                return context;
            }

            PlayerActionsInput.Enable();
            return PlayerActionsInput;
        }

        public InputContext PeekContext()
        {
            if (_contextStack.Count <= 0)
            {
                return PlayerActionsInput;
            }

            return _contextStack.Peek();
        }
    }
}