using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public abstract class Unit : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _currentHP = 100;
        [SerializeField] private int _maxHP = 100;

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

        public UnityEvent OnUnitDeath;

        protected BattleMap _battleMap;
        [Inject]
        public void Construct(BattleMap map)
        {
            _battleMap = map;
        }

        public abstract void ExecuteAction();
        public void TakeDamage(Damage damage)
        {
            CurrentHP -= damage.Amount;
            if (CurrentHP <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
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
    }
}
