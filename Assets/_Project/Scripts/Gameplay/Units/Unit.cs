using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class Unit : MonoBehaviour, IDamageable, IDamageSource
    {
        [SerializeField] private int _currentHP = 5;
        [SerializeField] private int _maxHP = 5;
        [SerializeField] private int _currentAttack = 5;
        [SerializeField] private int _maxAttack = 5;
        private List<UnitComponent> _unitComponents = new();
        private IObjectResolver _resolver;
        private bool _isShielded;
        private Vector2Int _position;
        private bool _isPositionDirty = true;
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

        public int CurrentHP
        {
            get => _currentHP;
            private set => _currentHP = Mathf.Clamp(value, 0, _maxHP);
        }

        public int MaxHP
        {
            get => _maxHP;
            private set => _maxHP = Mathf.Max(value, 0);
        }


        public int CurrentAttack
        {
            get => _currentAttack;
            private set => _currentAttack = Mathf.Clamp(value, 0, _maxAttack);
        }

        public int MaxAttack
        {
            get => _maxAttack;
            private set => _maxAttack = Mathf.Max(value, 0);
        }

        public UnityEvent OnUnitDeath;

        protected BattleMap _battleMap;
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

        public void ExecuteAction()
        {
            foreach (var component in _unitComponents)
            {
                component.OnExecuteAction();
            }
        }
        public void TakeDamage(Damage damage)
        {
            if (_isShielded)
            {
                _isShielded = false; // Щит ломается и поглощает весь урон
                return;
            }
            CurrentHP -= damage.Amount;
            if (CurrentHP <= 0)
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
            CurrentHP += amount;
        }
        public void BuffAttack(int amount)
        {
            CurrentAttack += amount;
        }

        public void DebuffAttack(int amount)
        {
            if (CurrentAttack - amount <= 0)
            {
                return;
            }
            CurrentAttack -= amount;
        }

        public void BuffMaxHP(int amount)
        {
            MaxHP += amount;
            CurrentHP += amount;
        }
    }
}
