using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    [AddComponentMenu("UnitComponents/HealSelfOnAction")]
    public class HealSelfOnActionUnitComponent : UnitComponent
    {
        [SerializeField] private int _healAmount = 3;
        public override void OnExecuteAction()
        {
            unit.Heal(_healAmount);
            Debug.Log($"{unit.name} восстановил(-а) {_healAmount} здоровья!");
        }
    }
}