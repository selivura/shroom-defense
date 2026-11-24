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
        private PlayerUIHandCardsController _handCardsController;

        [SerializeField] private int _currentHP = 100;
        [field:SerializeField] public StatController StatController { get; private set; } = new();
        [field:SerializeField] public int CurrentMana { get; private set; } = 0;
        [field:SerializeField] public int HandSize = 5;
       

        public IReadOnlyCollection<Card> Hand => _hand.AsReadOnly();

        public int CurrentHP
        {
            get => _currentHP;
        }


        [Inject]
        public void Construct(PersistentDeck persDeck, BattleMap map, TurnManager turnManager, CardFactory cardFactory, PlayerUIHandCardsController handCardsController)
        {
            _persistentDeck = persDeck;
            _battleMap = map;
            _turnManager = turnManager;
            _cardFactory = cardFactory;
            _handCardsController = handCardsController;

            _currentDeck = new Deck(_persistentDeck.DraftedDeck.CardPrefabs);
        }
        private void Start()
        {
            RefillHand();
        }

        public void TakeDamage(Damage damage)
        {
            _currentHP -= damage.Amount;
            if(_currentHP <= 0)
            {
                Die();
            }
        }

        private void RefillHand()
        {
            while(DrawRandomCardFromDeck(false))
            {
                // Continue drawing cards until the hand is full or the deck is empty
            }
            StringBuilder handContents = new StringBuilder("Hand contents:");
            foreach (var item in _hand)
            {
                handContents.AppendLine(item.name);
            }
            Debug.Log(handContents);

            _handCardsController?.SyncHand(_hand);
        }
        /// <summary>
        /// Дать игроку в руку определенную карту
        /// </summary>
        /// <param name="cardPrefab"></param>
        /// <param name="bypassMaxHandSize"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool DrawCard(Card cardPrefab, bool bypassMaxHandSize = false)
        {
            if (cardPrefab == null || !CanDrawCard(bypassMaxHandSize))
            {
                return false;
            }

            if (_cardFactory == null)
            {
                throw new NullReferenceException($"{name}: Card factory was not injected into player controller.");
            }

            _hand.Add(_cardFactory.Create(cardPrefab));
            _handCardsController?.SyncHand(_hand);
            return true;
        }

        public bool CanDrawCardFromDeck(bool bypassMaxHandSize = false)
        {
            return CanDrawCard(bypassMaxHandSize) && _currentDeck != null && _currentDeck.CardPrefabs.Count > 0;
        }
        public bool CanDrawCard(bool bypassMaxHandSize = false)
        {
            return (bypassMaxHandSize || _hand.Count < HandSize);
        }

        /// <summary>
        /// Дать игроку в руку случайную карту из колоды, если в ней остались карты
        /// </summary>
        /// <param name="bypassMaxHandSize"></param>
        /// <returns></returns>
        public bool DrawRandomCardFromDeck(bool bypassMaxHandSize = false)
        {
            if (!CanDrawCardFromDeck(bypassMaxHandSize))
            {
                return false;
            }

            int randomIndex = UnityEngine.Random.Range(0, _currentDeck.CardPrefabs.Count);

            Card cardPrefab = _currentDeck.CardPrefabs[randomIndex];
            if (!DrawCard(cardPrefab, bypassMaxHandSize))
            {
                return false;
            }
            _currentDeck.CardPrefabs.RemoveAt(randomIndex);
            return true;
        }

        public bool PlayCardFromHand(int index, CardTargetData targetData)
        {
            if (_turnManager.CurrentTurn != TurnType.PlayerAdvance)
            {
                return false;
            }
            if (index < 0 || index >= _hand.Count)
            {
                return false;
            }

            Card card = _hand[index];
            card.ExecuteAction(targetData);
            _hand.RemoveAt(index);
            Destroy(card.gameObject);
            RefillHand();
            return true;
        }

        public void EndTurn()
        {
            if(_currentHP <= 0)
            {
                Debug.LogWarning("Cannot end turn. Player is dead.");
                return;
            }
            if(_turnManager.CurrentTurn != TurnType.PlayerAdvance)
            {
                Debug.LogWarning("Cannot end turn. It's not the player's turn.");
                return;
            }
            _turnManager.FinishActions();
        }

        private void Die()
        {
            Debug.Log("Player has died. GAME OVER.");
        }
    }
}
