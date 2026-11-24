using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/MushroomCatcherOnAction")]
    public class MushroomCatcherUnit : UnitComponent
    {
        public override async UniTask OnExecuteAction()
        {
            var enemyUnits = unit.FindAllUnitsOnEnemyField();

            if (enemyUnits.Count > 0)
            {
                int randomIndex = Random.Range(0, enemyUnits.Count);
                Unit targetToEat = enemyUnits[randomIndex];

                targetToEat.TakeDamage(new Damage(unit, 9999));
                Debug.Log($"Гриболов съел {targetToEat.name}!");
            }
        }
    }
}