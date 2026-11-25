using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense.Gameplay
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private CardFactory _cardFactory;
        private DeckbuildResults _persistentDeck;
        private Deck _currentDeck;
        private List<Card> _cardsInHand = new();

        private BattleMap _battleMap;
        private TurnManager _turnManager;
        private PlayerUIHandCardsController _handCardsController;

        [SerializeField] private int _currentHP = 100;
        [field:SerializeField] public StatController StatController { get; private set; } = new();
        [SerializeField] [Required] private StatType _maxManaStatType;
        [SerializeField] private HeroTool _tool;
        private IObjectResolver _resolver;

        [field:SerializeField] public int CurrentMana { get; private set; } = 999;
        [field: SerializeField] public int StartHandSize { get; private set; } = 4;


        public IReadOnlyCollection<Card> Hand => _cardsInHand.AsReadOnly();

        public int CurrentHP
        {
            get => _currentHP;
        }


        [Inject]
        public void Construct(IObjectResolver resolver, DeckbuildResults persDeck, BattleMap map, TurnManager turnManager, CardFactory cardFactory, PlayerUIHandCardsController handCardsController)
        {
            _resolver = resolver;
            _persistentDeck = persDeck;
            _battleMap = map;
            _turnManager = turnManager;
            _cardFactory = cardFactory;
            _handCardsController = handCardsController;

            _currentDeck = new Deck(_persistentDeck.DraftedDeck.CardPrefabs);
        }

        private void OnEnable()
        {
            _turnManager.OnTurnChanged.AddListener(OnTurnChanged);
        }

        private void OnDisable()
        {
            _turnManager.OnTurnChanged.RemoveListener(OnTurnChanged);
        }

        private void OnTurnChanged(TurnType type)
        {
            if(type != TurnType.PlayerAdvance)
            {
                return;
            }

            DrawRandomCardFromDeck();
            if (_maxManaStatType == null)
            {
                throw new NullReferenceException("Max mana stat type is not assigned.");
            }
            if (!StatController.GetStatValue(_maxManaStatType, out float maxMana))
            {
                throw new NullReferenceException("Max mana stat type is not found in player's stat controller.");
            }
            CurrentMana = (int)maxMana;
        }

        private void Start()
        {
            for (int i = 0; i < StartHandSize; i++)
            {
                DrawRandomCardFromDeck();
            }
        }

        public void TakeDamage(Damage damage)
        {
            _currentHP -= damage.Amount;
            if(_currentHP <= 0)
            {
                Die();
            }
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
            if (cardPrefab == null)
            {
                return false;
            }

            if (_cardFactory == null)
            {
                throw new NullReferenceException($"{name}: Card factory was not injected into player controller.");
            }

            _cardsInHand.Add(_cardFactory.Create(cardPrefab));
            _handCardsController?.SyncHand(_cardsInHand);
            return true;
        }

        public bool CanDrawCardFromDeck()
        {
            return _currentDeck != null && _currentDeck.CardPrefabs.Count > 0;
        }

        /// <summary>
        /// Дать игроку в руку случайную карту из колоды, если в ней остались карты
        /// </summary>
        /// <param name="bypassMaxHandSize"></param>
        /// <returns></returns>
        public bool DrawRandomCardFromDeck()
        {
            if (!CanDrawCardFromDeck())
            {
                return false;
            }

            int randomIndex = UnityEngine.Random.Range(0, _currentDeck.CardPrefabs.Count);

            Card cardPrefab = _currentDeck.CardPrefabs[randomIndex];
            if (!DrawCard(cardPrefab))
            {
                return false;
            }
            _currentDeck.CardPrefabs.RemoveAt(randomIndex);
            return true;
        }

        public PlayerCardPlayResult PlayCardFromHand(int index, CardTargetData targetData)
        {
            if(targetData == null)
            {
                throw new ArgumentNullException(nameof(targetData), "Target data cannot be null when playing a card.");
            }
            if (_turnManager.CurrentTurn != TurnType.PlayerAdvance)
            {
                return PlayerCardPlayResult.NotPlayerTurn;
            }
            if (index < 0 || index >= _cardsInHand.Count)
            {
                return PlayerCardPlayResult.Fail;
            }

            Card card = _cardsInHand[index];
            if(CurrentMana < card.ManaCost)
            {
                return PlayerCardPlayResult.NotEnoughMana;
            }

            if (!card.ExecuteAction(targetData))
            {
                return PlayerCardPlayResult.Fail;
            }

            CurrentMana -= card.ManaCost;
            _cardsInHand.RemoveAt(index);
            Destroy(card.gameObject);
            _handCardsController.SyncHand(_cardsInHand);
            return PlayerCardPlayResult.Success;
        }

        public void DropCardFromHand(int index)
        {
            if (_cardsInHand.Count <= index) return;
            _cardsInHand.RemoveAt(index);
            _handCardsController.SyncHand(_cardsInHand);
        }

        public void DropRandomCardFromHand()
        {
            int randomIndex = UnityEngine.Random.Range(0, _cardsInHand.Count);
            DropCardFromHand(randomIndex);
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

        public void UseTool()
        {
            if (_tool == null)
            {
                Debug.LogWarning("No tool assigned to player.");
                return;
            }
            if (CurrentMana < _tool.ManaCost)
            {
                Debug.LogWarning("Not enough mana to use the tool.");
                return;
            }
            if(_turnManager.CurrentTurn != TurnType.PlayerAdvance)
            {
                Debug.LogWarning("Cannot use tool. It's not the player's turn.");
                return;
            }
            _tool.OnSuccessfullyUsed.AddListener(OnToolSuccessfulyUsed);
            _tool.OnCancelledUse.AddListener(OnToolCancelledUse);
            _tool.UseTool();
        }

        public void CreateAndAssignTool(HeroTool toolPrefab)
        {
            _tool = _resolver.Instantiate(toolPrefab);
        }

        private void OnToolSuccessfulyUsed()
        {
            CurrentMana -= _tool.ManaCost;
            _tool.OnSuccessfullyUsed.RemoveListener(OnToolSuccessfulyUsed);
            _tool.OnSuccessfullyUsed.RemoveListener(OnToolCancelledUse);
            Debug.Log("COCK");
        }

        private void OnToolCancelledUse()
        {
            _tool.OnSuccessfullyUsed.RemoveListener(OnToolSuccessfulyUsed);
            _tool.OnSuccessfullyUsed.RemoveListener(OnToolCancelledUse);
        }

        private void Die()
        {
            Debug.Log("Player has died. GAME OVER.");
        }
    }

    public enum PlayerCardPlayResult
    {
        Success,
        NotPlayerTurn,
        NotEnoughMana,
        Fail
    }
}
