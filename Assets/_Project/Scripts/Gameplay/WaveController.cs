using System;
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

        private int _currentWaveDuration = 0;
        private List<Unit> _currentWaveUnits = new();
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


        // 0. Update wave duration counter
        // 1. Check if wave duration is over OR all enemies on field are dead. If not return
        // 2. Increment wave index
        // 3. Is the wave index valid? If not, end the game
        // 4. Spawn enemies for the current wave
        // 5. Update wave duration counter
        private void OnTurnChanged(TurnType turnType)
        {
            if (turnType != TurnType.EnemyAdvance) return;
            _turnManager.EnemyAdvance();
            Debug.Log("Enemy advanced...");

            _currentWaveDuration--;

            if (_enemiesForSpawnQueue.Count > 0)
            {
                TrySpawningQueuedEnemies();
                _turnManager.FinishActions();
                return;
            }
            RefreshUnitList();
            if (_currentWaveDuration > 0 && _currentWaveUnits.Count > 0)
            {
                _turnManager.FinishActions();
                return;
            }

            CurrentWaveIndex++;

            if (CurrentWaveIndex >= LevelData.Waves.Count)
            {
                Debug.Log("All waves were passed! PLAYER VICTORY!");
                _turnManager.FinishActions();
                return;
            }
            Debug.Log("NEW WAVE!");

            _currentWaveDuration = LevelData.Waves[CurrentWaveIndex].WaveDuration;

            foreach (var unitPrefab in LevelData.Waves[CurrentWaveIndex].UnitPrefabs)
            {
                _enemiesForSpawnQueue.Enqueue(unitPrefab);
            }

            TrySpawningQueuedEnemies();
            RefreshUnitList();
            _turnManager.FinishActions();
        }

        private void TrySpawningQueuedEnemies()
        {
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
                _currentWaveUnits.Add(createdUnit);
            }
        }

        private void RefreshUnitList()
        {
            var aliveUnits = new List<Unit>();
            foreach (var unit in _currentWaveUnits)
            {
                if(unit == null)
                {
                    continue;
                }
                if(unit.CurrentHP <= 0)
                {
                    continue;
                }
                aliveUnits.Add(unit);
            }
            _currentWaveUnits = aliveUnits;
        }
    }
}
