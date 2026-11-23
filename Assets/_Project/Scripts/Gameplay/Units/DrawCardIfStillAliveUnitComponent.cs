using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/DrawCardIfStillAlive")]
    public class DrawCardIfStillAliveUnitComponent : UnitComponent
    {
        public override void OnExecuteAction()
        {
            if (unit.CurrentHP > 0)
            {
                // Добор карты 
            }
            Debug.Log("ForgottenLoafUnit unit executed action"); 
        }
    }
}

