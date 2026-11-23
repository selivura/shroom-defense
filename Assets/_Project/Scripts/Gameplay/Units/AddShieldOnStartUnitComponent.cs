using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HeavyPrivateUnit")]
    public class AddShieldOnStartUnitComponent : UnitComponent
    {
        private void Start()
        {
            unit.AddShield();
        }
    }
}

