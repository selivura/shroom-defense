using Cysharp.Threading.Tasks;
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
        [SerializeField] private float _actionExecutionDelay = .5f;

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
                    UniTask.Void(() => ExecuteTurn(false));
                    break;
                case TurnType.PlayerExecute:
                    SetTurn(TurnType.EnemyExecute);
                    UniTask.Void(() => ExecuteTurn(true));
                    break;
                case TurnType.EnemyExecute:
                    SetTurn(TurnType.EnemyAdvance);
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
        }

        private async UniTaskVoid ExecuteTurn(bool isEnemyTurn)
        {
            foreach (var row in _battleMap.Rows)
            {
                var unitsSnapshot = new Dictionary<int, Unit>(row.Units);
                foreach (var pair in unitsSnapshot)
                {
                    if (isEnemyTurn)
                    {
                        if (pair.Key <= 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (pair.Key >= 0)
                        {
                            continue;
                        }
                    }

                    await pair.Value.ExecuteAction();
                    await UniTask.WaitForSeconds(_actionExecutionDelay);
                }
            }
            _enemyExecuteEndChannel.Invoke(new());
            FinishActions();
        }
    }
}
