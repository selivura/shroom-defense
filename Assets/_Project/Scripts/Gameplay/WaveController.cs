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

        [field: SerializeField] public int CurrentWaveIndex { get; private set; } = 0;

        private Queue<Unit> _enemiesForSpawnQueue = new();

        [Inject]
        public void Construct(TurnManager turnManager, BattleMap battleMap, UnitFactory unitFactory)
        {
            _unitFactory = unitFactory;
            _turnManager = turnManager;
            _battleMap = battleMap;

            _turnManager.OnTurnChanged.AddListener(OnTurnChanged);
            CurrentWaveIndex = -1;
        }

        private void OnTurnChanged(TurnType turnType)
        {
            if (turnType != TurnType.EnemyAdvance) return;
            _turnManager.EnemyAdvance();


            if (_enemiesForSpawnQueue.Count <= 0)
            {
                CurrentWaveIndex++;
                if (CurrentWaveIndex >= LevelData.Waves.Count)
                {
                    Debug.Log("All waves were passed!");
                }
                else foreach (var unitPrefab in LevelData.Waves[CurrentWaveIndex].UnitPrefabs)
                {
                    _enemiesForSpawnQueue.Enqueue(unitPrefab);
                }
            }

            if (_enemiesForSpawnQueue.Count <= 0)
            {
                Debug.Log("All waves enemies are spawned!");
                _turnManager.FinishActions();
                return;
            }

            for (int i = 0; i < LevelData.RowsNumber; i++)
            {
                if (_enemiesForSpawnQueue.Count <= 0)
                {
                    break;
                }

                int column = LevelData.FurthestEnemyColumn;
                if (_battleMap.Rows[i].Units.ContainsKey(column))
                {
                    continue;
                }
                var prefab = _enemiesForSpawnQueue.Dequeue();
                if (prefab == null)
                {
                    continue;
                }
                var createdUnit = _unitFactory.Create(prefab);
                _battleMap.PlaceUnitAt(new Vector2Int(column, i), createdUnit);
            }
            _turnManager.FinishActions();
        }
    }
}
