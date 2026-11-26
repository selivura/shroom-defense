using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/TainstvoVorona")]
    public class TainstvoVoronaUnitComponent : UnitComponent
    {
        private BattleMap _battleMap;
        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
        public override async UniTask DoComponentAction()
        {
            var enemies = unit.FindAllUnitsOnEnemyField();
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(new Damage(unit, _battleMap.TotalVictimsCount));
            }
            Debug.Log($"{unit.name} damaged everyone on enemy team (ExecuteAction)");
        }
    }
}