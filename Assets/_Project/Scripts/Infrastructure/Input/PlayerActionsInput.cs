using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShroomDefense.Input
{
    public class PlayerActionsInput : InputContext, GameInputActions.IPlayerActions
    {
        public PlayerActionsInput(GameInputActions actions) : base(actions)
        {
        }
        // Добавить бинды на что-нибудь здесь
        internal override void Enable()
        {
            actions.Player.Enable();
        }

        internal override void Disable()
        {
            actions.Player.Disable();
        }
    }
}