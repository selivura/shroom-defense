using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private CardFactory _cardFactory;
        private PersistentDeck _persistentDeck;
        private Deck _currentDeck;
        private List<Card> _hand = new();

        private BattleMap _battleMap;
        private TurnManager _turnManager;
        [SerializeField] private int _currentHP = 100;
        [SerializeField] private int _maxHP = 100;
        [field:SerializeField] public int CurrentMana { get; private set; } = 0;
        [field:SerializeField] public int MaxMana { get; private set; } = 0;
        [field:SerializeField] public int HandSize = 5;

        public IReadOnlyCollection<Card> Hand => _hand.AsReadOnly();

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


        [Inject]
        public void Construct(PersistentDeck persDeck, BattleMap map, TurnManager turnManager, CardFactory cardFactory)
        {
            _persistentDeck = persDeck;
            _battleMap = map;
            _turnManager = turnManager;
            _cardFactory = cardFactory;

            _currentDeck = new Deck(_persistentDeck.DraftedDeck.CardPrefabs);
            RefillHand();
        }

        public void TakeDamage(Damage damage)
        {
            CurrentHP -= damage.Amount;
            if(CurrentHP <= 0)
            {
                Die();
            }
        }

        private void RefillHand()
        {
            while(_hand.Count < HandSize && _currentDeck.CardPrefabs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, _currentDeck.CardPrefabs.Count);
                Card drawnCard = _currentDeck.CardPrefabs[randomIndex];

                _hand.Add(_cardFactory.Create(drawnCard));
                _currentDeck.CardPrefabs.RemoveAt(randomIndex);
            }
            StringBuilder handContents = new StringBuilder("Hand contents:");
            foreach (var item in _hand)
            {
                handContents.AppendLine(item.name);
            }
            Debug.Log(handContents);
        }

        public void PlayCardFromHand(int index, CardTargetData targetData)
        {
            if (index >= 0 && index < _hand.Count)
            {
                Card card = _hand[index];
                card.ExecuteAction(targetData);
                _hand.RemoveAt(index);
            }
            RefillHand();
        }

        // TODO Удалить и сделать нормальный выбор карты и таргета
        public void PlayFirstCardFromHand()
        {
            if (_hand.Count == 0)
            {
                return;
            }

            List<Vector2Int> availablePlayerPositions = new();
            for (int y = 0; y < _battleMap.Rows.Count; y++)
            {
                if (!_battleMap.Rows[y].Units.ContainsKey(-1))
                {
                    availablePlayerPositions.Add(new Vector2Int(-1, y));
                }
            }

            if (availablePlayerPositions.Count == 0)
            {
                return;
            }

            int randomIndex = UnityEngine.Random.Range(0, availablePlayerPositions.Count);
            var data = new CardTargetData
            {
                MapPostion = availablePlayerPositions[randomIndex]
            };
            PlayCardFromHand(0, data);
        }

        public void EndTurn()
        {
            if(_turnManager.CurrentTurn != TurnType.PlayerAdvance)
            {
                Debug.LogWarning("Cannot end turn. It's not the player's turn.");
                return;
            }
            _turnManager.FinishActions();
        }

        private void Die()
        {
            Debug.Log("Player has died.");
        }
    }
}
