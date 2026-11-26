using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/MushroomCatcher")]
    public class MushroomCatcherUnitComponent : UnitComponent
    {
        public override async UniTask DoComponentAction()
        {
            var enemyUnits = unit.FindAllUnitsOnEnemyField();

            if (enemyUnits.Count > 0)
            {
                int randomIndex = Random.Range(0, enemyUnits.Count);
                Unit targetToEat = enemyUnits[randomIndex];
                targetToEat.Instakill(new Damage(unit, 0));
                Debug.Log($"Гриболов съел {targetToEat.name}!");
            }
        }
    }
}