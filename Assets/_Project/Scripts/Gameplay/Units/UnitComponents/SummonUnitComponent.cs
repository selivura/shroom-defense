using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/Summon")]
    public class SummonUnitComponent : UnitComponent
    {
        [SerializeField, Required] private Unit _unitPrefab;

        private UnitFactory _factory;
        private BattleMap _battleMap;
        [Inject]
        public void Construct(UnitFactory factory, BattleMap battleMap)
        {
            _factory = factory;
            _battleMap = battleMap;
        }

        public override async UniTask DoComponentAction()
        {
            Vector2Int? emptyCell = FindEmptyEnemyCell();
            if (_unitPrefab == null)
            {
                Debug.LogException(new InvalidOperationException($"{name} has no unit prefab assigned."), this);
                return;
            }
            if (!emptyCell.HasValue)
            {
                Debug.Log($"{unit.name} couldn't summon {_unitPrefab.name}: no empty enemy cell.");
                return;
            }

            var summonedUnit = _factory.Create(_unitPrefab);
            _battleMap.PlaceUnitAt(emptyCell.Value, summonedUnit);
            Debug.Log($"{unit.name} summoned {summonedUnit.name}!");
        }

        private Vector2Int? FindEmptyEnemyCell()
        {
            int enemySide = unit.Position.x < 0 ? 1 : -1;
            int limit = Mathf.Max(Mathf.Abs(_battleMap.MinBounds.x), Mathf.Abs(_battleMap.MaxBounds.x));

            for (int y = 0; y < _battleMap.Rows.Count; y++)
            {
                for (int distance = 1; distance <= limit; distance++)
                {
                    int x = enemySide * distance;
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