using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/SelfDamage")]
    public class SelfDamageUnitComponent : UnitComponent
    {
        [SerializeField] private int _damageAmount = 2;
        public override async UniTask DoComponentAction()
        {
            unit.TakeDamage(new Damage(unit, _damageAmount));
        }
    }
}

