using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/AddShieldOnStart")]
    public class AddShieldOnStartUnitComponent : UnitComponent
    {
        protected override void OnComponentInit()
        {
            unit.AddShield();
        }
    }
}

