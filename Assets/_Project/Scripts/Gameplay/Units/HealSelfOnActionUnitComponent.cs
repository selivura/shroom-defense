using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HealSelfOnAction")]
    public class HealSelfOnActionUnitComponent : UnitComponent
    {
        [SerializeField] private int _healAmount = 3;
        public override async UniTask OnExecuteAction()
        {
            unit.Heal(_healAmount);
            Debug.Log($"{unit.name} восстановил(-а) {_healAmount} здоровья!");
        }
    }
}