using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class WaveController : MonoBehaviour
    {
        private TurnManager _turnManager;
        private BattleMap _battleMap;
        private UnitFactory _unitFactory;
        public LevelData LevelData;

        [field:SerializeField] public int CurrentWaveIndex { get; private set; } = 0;

        private Queue<Unit> _enemyQueue = new();

        [Inject]
        public void Construct(TurnManager turnManager, BattleMap battleMap, UnitFactory unitFactory)
        {
            _unitFactory = unitFactory;
            _turnManager = turnManager;
            _battleMap = battleMap;

            _turnManager.OnTurnChanged.AddListener(OnTurnChanged);
        }

        private void OnTurnChanged(TurnType turnType)
        {
            if(turnType != TurnType.EnemyAdvance) return;

            foreach (var unitPrefab in LevelData.Waves[CurrentWaveIndex].UnitPrefabs)
            {
                _enemyQueue.Enqueue(unitPrefab);
            }

            for (int i = 0; i < LevelData.RowsNumber; i++)
            {
                if (_enemyQueue.Count <= 0)
                {
                    break;
                }

                int column = LevelData.FurthestEnemyColumn + 1;
                if (_battleMap.Rows[i].Units.ContainsKey(column))
                {
                    continue;
                }
                var createdUnit = _unitFactory.Create(_enemyQueue.Dequeue());
                _battleMap.PlaceUnitAt(new Vector2Int(column, i), createdUnit);
            }
            _turnManager.FinishActions();
        }
    }
}
