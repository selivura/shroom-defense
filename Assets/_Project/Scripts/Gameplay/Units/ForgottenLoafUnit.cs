using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class ForgottenLoafUnit : Unit
    {
        public override void ExecuteAction()
        {
            TakeDamage(new Damage(this, 2));
            if (CurrentHP > 0)
            {
                // Добор карты 
            }
            Debug.Log("ForgottenLoafUnit unit executed action"); 
        }
    }
}

