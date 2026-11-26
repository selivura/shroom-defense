using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/AddShield")]
    public class AddShieldtUnitComponent : UnitComponent
    {
        public override async UniTask DoComponentAction()
        {
            unit.AddShield();
        }
    }
}

