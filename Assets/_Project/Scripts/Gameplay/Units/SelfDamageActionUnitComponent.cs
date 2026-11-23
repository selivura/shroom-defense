using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/SelfDamageOnAction")]
    public class SelfDamageActionUnitComponent : UnitComponent
    {
        [SerializeField] private int _damageAmount = 2;
        public override void OnExecuteAction()
        {
            unit.TakeDamage(new Damage(unit, _damageAmount));
        }
    }
}

