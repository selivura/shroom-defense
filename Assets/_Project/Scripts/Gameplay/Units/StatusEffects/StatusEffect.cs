using System;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [Serializable]
    public abstract class StatusEffect
    {
        public int DurationTurns = 0;

        public Sprite Icon;
        public string DisplayName;
        public string Description;

        protected Unit unit;

        public StatusEffect(int duration)
        {
            DurationTurns = duration;
        }

        public void Apply(Unit targetUnit)
        {
            unit = targetUnit;
            OnApply();
        }
        protected virtual void OnApply()
        {

        }

        public virtual void OnUnitBeforeActionsExecuted()
        {

        }

        public virtual void OnUnitAfterActionsExecuted()
        {

        }

        public virtual void OnExpire()
        {

        }

        /// <summary>
        /// Определяет поведение эффекта при наложении нового экземпляра того же эффекта.
        /// По умолчанию ничего не происходит.
        /// </summary>
        /// <param name="newInstance"></param>
        /// <returns>true, если новый наложенный эффект нужно удалить, false если оставить</returns>
        public virtual bool HandleAnotherInstance(StatusEffect newInstance)
        {
            return false;
        }
    }
}
