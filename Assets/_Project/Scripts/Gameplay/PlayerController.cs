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

            _handCardsController?.SyncHand(_hand);
        }

        public bool PlayCardFromHand(int index, CardTargetData targetData)
        {
            if (_turnManager.CurrentTurn != TurnType.PlayerAdvance)
            {
                return false;
            }
            if (index >= 0 && index < _hand.Count)
            {
                Card card = _hand[index];
                card.ExecuteAction(targetData);
                _hand.RemoveAt(index);
            }
            RefillHand();
            return true;
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
