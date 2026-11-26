using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HealSelf")]
    public class HealSelfUnitComponent : UnitComponent
    {
        [SerializeField] private int _healAmount = 3;
        public override async UniTask DoComponentAction()
        {
            unit.Heal(_healAmount);
            Debug.Log($"{unit.name} восстановил(-а) {_healAmount} здоровья!");
        }
    }
}