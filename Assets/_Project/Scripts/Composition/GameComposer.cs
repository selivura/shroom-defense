using Cysharp.Threading.Tasks;
using ShroomDefense.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace ShroomDefense
{
    public class GameComposer : IStartable
    {
        private TurnManager _turnManager;
        private WaveController _waveController;
        private BattleMap _battleMap;

        public GameComposer(TurnManager turnManager, WaveController waveController, BattleMap battleMap)
        {
            _turnManager = turnManager;
            _waveController = waveController;
            _battleMap = battleMap;
        }
        public void Start()
        {
            UniTask.Void(() => StartGameSequence());
        }

        private async UniTaskVoid StartGameSequence()
        {
            _battleMap.Rows = new List<Row>();
            for (int i = 0; i < _waveController.LevelData.RowsNumber; i++)
            {
                _battleMap.Rows.Add(new());
            }
            _battleMap.MaxBounds = new Vector2Int(_waveController.LevelData.FurthestEnemyColumn, _waveController.LevelData.RowsNumber - 1);
            _battleMap.MinBounds = new Vector2Int(_waveController.LevelData.FurthestPlayerColumn, 0);
            _battleMap.GenerateMap();

            Debug.Log("3");
            await UniTask.WaitForSeconds(1);
            Debug.Log("2");
            await UniTask.WaitForSeconds(1);
            Debug.Log("1");
            await UniTask.WaitForSeconds(1);
            _turnManager.StartGame();
        }
    }
}
