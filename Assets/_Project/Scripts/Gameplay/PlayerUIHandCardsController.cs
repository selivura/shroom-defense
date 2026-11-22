using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense.Gameplay
{
    public class PlayerUIHandCardsController : MonoBehaviour
    {
        [SerializeField] private HandCardDraggable _draggablePrefab;
        [SerializeField] private float _cardSpacing = 2.5f;
        [SerializeField] private PlayerController _playerController;

        private readonly List<HandCardDraggable> _draggableCards = new();
        private BattleMap _map;
        private IObjectResolver _resolver;

        [Inject]
        public void Construct(BattleMap map, IObjectResolver resolver)
        {
            _map = map;
            _resolver = resolver;
        }

        public void SyncHand(IReadOnlyCollection<Card> hand)
        {
            int handCount = hand.Count;
            while (_draggableCards.Count < handCount)
            {
                var spawnedCard = _resolver.Instantiate(_draggablePrefab);
                spawnedCard.transform.SetParent(transform, false);
                _draggableCards.Add(spawnedCard);
            }

            while (_draggableCards.Count > handCount)
            {
                int lastIndex = _draggableCards.Count - 1;
                Destroy(_draggableCards[lastIndex].gameObject);
                _draggableCards.RemoveAt(lastIndex);
            }

            RefreshLayout(hand);
        }

        public bool TryPlayCard(int cardIndex, CardTargetData targetData)
        {
            if (!_map.ValidatePosition(targetData.MapPostion))
            {
                if (cardIndex >= 0 && cardIndex < _draggableCards.Count)
                {
                    _draggableCards[cardIndex].CancelDrag();
                }
                return false;
            }

            return _playerController.PlayCardFromHand(cardIndex, targetData);
        }

        private void RefreshLayout(IReadOnlyCollection<Card> hand)
        {
            float centerOffset = (_draggableCards.Count - 1) * _cardSpacing * 0.5f;
            int i = 0;

            foreach (Card card in hand)
            {
                var draggableCard = _draggableCards[i];
                draggableCard.gameObject.SetActive(true);
                draggableCard.SetCard(i, card);
                draggableCard.transform.localPosition = new Vector3(i * _cardSpacing - centerOffset, 0f, 0f);
                i++;
            }
        }
    }
}
