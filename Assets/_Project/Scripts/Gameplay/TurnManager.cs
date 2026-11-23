using NaughtyAttributes;
using ShroomDefense.EventChannels;
using ShroomDefense.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace ShroomDefense
{
    public enum TurnType
    {
        EnemyAdvance,
        PlayerAdvance,
        PlayerExecute,
        EnemyExecute
    }

    public class TurnManager : MonoBehaviour
    {
        private BattleMap _battleMap;
        [field:SerializeField] [field:ReadOnly] public TurnType CurrentTurn { get; private set; } = TurnType.EnemyAdvance;
        public UnityEvent<TurnType> OnTurnChanged;

        [SerializeField] [Required] private EventChannel _playerExecuteEndChannel;
        [SerializeField] [Required] private EventChannel _enemyExecuteEndChannel;

        [Inject]
        public void Construct(BattleMap map)
        {
            _battleMap = map;
        }

        public void StartGame()
        {
            SetTurn(TurnType.EnemyAdvance);
        }

        /// <summary>
        /// Advances the game to the next turn phase and executes the appropriate turn actions.
        /// </summary>
        public void FinishActions()
        {
            switch (CurrentTurn)
            {
                case TurnType.EnemyAdvance:
                    SetTurn(TurnType.PlayerAdvance);
                    Debug.Log("Player advance turn!");
                    break;
                case TurnType.PlayerAdvance:
                    SetTurn(TurnType.PlayerExecute);
                    ExecutePlayerTurn();
                    break;
                case TurnType.PlayerExecute:
                    SetTurn(TurnType.EnemyExecute);
                    ExecuteEnemyTurn();
                    break;
                case TurnType.EnemyExecute:
                    SetTurn(TurnType.EnemyAdvance);
                    Debug.Log("Enemy advance turn!");
                    break;
                default:
                    break;
            }
        }

        private void SetTurn(TurnType turnType)
        {
            CurrentTurn = turnType;
            OnTurnChanged.Invoke(CurrentTurn);
        }

        public void EnemyAdvance()
        {
            List<AdvanceUnitComponent> unitsToAdvance = new List<AdvanceUnitComponent>();
            foreach (var row in _battleMap.Rows)
            {
                foreach (var pair in row.Units)
                {
                    if (pair.Key <= 0)
                    {
                        continue;
                    }
                    if(pair.Value.TryGetComponent(out AdvanceUnitComponent advanceUnitComponent))
                    {
                        unitsToAdvance.Add(advanceUnitComponent);
                    }
                }
            }
            foreach (var unit in unitsToAdvance)
            {
                unit.Advance();
            }
            Debug.Log("Enemy turn advance...");
        }

        private void ExecutePlayerTurn()
        {
            // TODO добавть асинк чтобы все не моментально просиходило
            foreach (var row in _battleMap.Rows)
            {
                foreach (var pair in row.Units)
                {
                    if(pair.Key >=0)
                    {
                        continue;
                    }
                    pair.Value.ExecuteAction();
                }
            }
            _playerExecuteEndChannel.Invoke(new());
            Debug.Log("Player turn execute...");
            FinishActions();
        }

        private void ExecuteEnemyTurn()
        {
            // TODO добавть асинк чтобы все не моментально просиходило
            foreach (var row in _battleMap.Rows)
            {
                foreach (var pair in row.Units)
                {
                    if (pair.Key <= 0)
                    {
                        continue;
                    }
                    pair.Value.ExecuteAction();
                }
            }
            _enemyExecuteEndChannel.Invoke(new());
            Debug.Log("Enemy turn execute...");
            FinishActions();
        }
    }
}
