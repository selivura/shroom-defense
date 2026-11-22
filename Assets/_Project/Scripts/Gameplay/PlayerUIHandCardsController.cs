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
            foreach (var draggableCard in _draggableCards)
            {
                draggableCard.gameObject.SetActive(false);
            }

            int i = 0;
            float centerOffset = (hand.Count - 1) * _cardSpacing * 0.5f;
            foreach (Card card in hand)
            {
                HandCardDraggable draggableCard = null;

                for (int poolIndex = 0; poolIndex < _draggableCards.Count; poolIndex++)
                {
                    if (!_draggableCards[poolIndex].gameObject.activeSelf)
                    {
                        draggableCard = _draggableCards[poolIndex];
                        break;
                    }
                }

                if (draggableCard == null)
                {
                    draggableCard = _resolver.Instantiate(_draggablePrefab);
                    draggableCard.transform.SetParent(transform, false);
                    _draggableCards.Add(draggableCard);
                }

                draggableCard.gameObject.SetActive(true);
                draggableCard.SetCard(i, card);
                draggableCard.transform.localPosition = new Vector3(i * _cardSpacing - centerOffset, 0f, 0f);
                i++;
            }
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

    }
}
