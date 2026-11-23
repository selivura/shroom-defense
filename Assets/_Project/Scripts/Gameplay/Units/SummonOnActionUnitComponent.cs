using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/SummonUnit")]
    public class SummonOnActionUnitComponent : UnitComponent
    {
        [SerializeField] private Unit _caterpillarPrefab;

        private UnitFactory _factory;
        private BattleMap _battleMap;
        [Inject]
        public void Construct(UnitFactory factory, BattleMap battleMap)
        {
            _factory = factory;
            _battleMap = battleMap;
        }

        public override void OnExecuteAction()
        {
            Vector2Int? emptyCell = FindEmptyEnemyCell();
            if (emptyCell.HasValue && _caterpillarPrefab != null)
            {
                var summonedUnit = _factory.Create(_caterpillarPrefab);
                _battleMap.PlaceUnitAt(emptyCell.Value, summonedUnit);
                Debug.Log($"{unit.name} призвал(-а) {summonedUnit.name}!");
            }
            else
            {
                Debug.Log($"{unit.name} не смог(-ла) призвать {_caterpillarPrefab.name}!");
            }
        }

        private Vector2Int? FindEmptyEnemyCell()
        {
            for (int y = 0; y < _battleMap.Rows.Count; y++)
            {
                for (int x = 1; x <= 3; x++)
                {
                    if (!_battleMap.Rows[y].Units.ContainsKey(x))
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return null; 
        }
    }
}