using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class Unit : MonoBehaviour, IDamageable, IDamageSource
    {
        [SerializeField] private int _currentHP = 5;
        [SerializeField] private float _delayBetweenActions = 0.5f;
        [field: SerializeField] public StatController StatController;
        private List<UnitComponent> _unitComponents = new();
        private IObjectResolver _resolver;
        private bool _isShielded;
        private Vector2Int _position;
        private bool _isPositionDirty = true;
        protected BattleMap _battleMap;
        
        [CreateProperty]
        public int CurrentHP
        {
            get => _currentHP;
        }
        public UnitFaction Faction;
        public Vector2Int Position
        {
            get
            {
                if(_isPositionDirty)
                {
                    var rows = _battleMap.Rows;
                    int x = 0;
                    int y = 0;
                    for (int i = 0; i < rows.Count; i++)
                    {
                        if (rows[i].Units.ContainsValue(this))
                        {
                            x = rows[i].Units.FirstOrDefault(x => x.Value == this).Key;
                            y = i;
                            break;
                        }
                    }
                    _isPositionDirty = false;
                    _position = new Vector2Int(x, y);
                }
                return _position;
            }
        }

        public UnityEvent OnUnitDeath;
        public UnityEvent OnBeforeExecutingActions;
        public UnityEvent OnAfterExecutingActions;


        [Inject]
        public void Construct(BattleMap map, IObjectResolver resolver)
        {
            _battleMap = map;
            _resolver = resolver;
            var unitComponents = GetComponents<UnitComponent>().ToList();
            foreach (var component in unitComponents)
            {
                RegisterComponent(component);
            }
        }

        /// <summary>
        /// Этот метод предназначен для регистрации компонентов добавленных после запуска игры.
        /// </summary>
        /// <param name="component"></param>
        public void RegisterComponent(UnitComponent component)
        {
            if (_unitComponents.Contains(component))
            {
                Debug.LogWarning($"Component {component.GetType().Name} is already registered to unit {name}");
                return;
            }
            _resolver.Inject(component);
            component.InitializeComponent(this);
            _unitComponents.Add(component);
        }

        /// <summary>
        /// В компонентах вызывается <see cref="UnitComponent.OnExecuteAction"/> в порядке в котором они расположены в инспекторе
        /// </summary>
        public async UniTask ExecuteAction()
        {
            OnBeforeExecutingActions.Invoke();
            foreach (var component in _unitComponents)
            {
                await component.OnExecuteAction();
                await UniTask.WaitForSeconds(_delayBetweenActions);
            }
            OnAfterExecutingActions.Invoke();
            await UniTask.WaitForSeconds(_delayBetweenActions);
        }
        public void TakeDamage(Damage damage)
        {
            if (_isShielded)
            {
                _isShielded = false; // Щит ломается и поглощает весь урон
                return;
            }
            _currentHP -= damage.Amount;
            if (_currentHP <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _battleMap.OnUnitDied(Position);
            _battleMap.ClearCell(Position);
            gameObject.SetActive(false);
            OnUnitDeath.Invoke();
            Debug.Log($"Unit {name} has died.");
        }

        public void Move(Vector2Int newCoords)
        {
            if(!_battleMap.ValidatePosition(newCoords))
            {
                throw new InvalidOperationException($"Cannot move unit {name} to {newCoords}. Position is invalid or occupied.");
            }

            _battleMap.ClearCell(Position);
            _battleMap.PlaceUnitAt(newCoords, this);

            _isPositionDirty = true;
        }

        public bool IsShielded => _isShielded;
        public void AddShield()
        {
            _isShielded = true;
        }

        public void Heal(int amount)
        {
            if (CurrentHP <= 0)
            {
                return; 
            }
            _currentHP += amount;
        }


        // Helper methods
        public List<Unit> FindAllUnitsOnMyTeam()
        {
            return FindAllUnitsOnTeam(Position.x < 0 ? -1 : 1);
        }

        public List<Unit> FindAllUnitsOnEnemyField()
        {
            return FindAllUnitsOnTeam(Position.x < 0 ? 1 : -1);
        }

        public List<Unit> FindAllUnitsOnTeam(int team)
        {
            if (team == 0)
            {
                throw new ArgumentException($"Team can't be ZERO!", nameof(team));
            }
            List<Unit> output = new List<Unit>();
            foreach (var row in _battleMap.Rows)
            {
                var unitsInRow = row.Units.Values
                    .Where(candidate => team * candidate.Position.x > 0)
                    .ToList();
                foreach (var unitInRow in unitsInRow)
                {
                    output.Add(unitInRow);
                }
            }
            return output;
        }
    }
}
