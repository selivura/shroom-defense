using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class HandCardDraggable : MonoBehaviour, IDraggable
    {
        [SerializeField] private int _cardIndex;
        [SerializeField] private SpriteRenderer _iconRenderer;
        private PlayerUIHandCardsController _handCardsController;

        [Inject]
        public void Construct(PlayerUIHandCardsController handCardsController)
        {
            _handCardsController = handCardsController;
        }
        private Vector2 _originalPosition;
        public void SetCardIndex(int index)
        {
            _cardIndex = index;
        }

        public void SetCard(int index, Card card)
        {
            SetCardIndex(index);
            if (_iconRenderer != null)
            {
                _iconRenderer.sprite = card?.CardIcon;
            }
        }
        public void BeginDrag()
        {
            _originalPosition = transform.position;
        }

        public void CancelDrag()
        {
            transform.position = _originalPosition;
        }

        public void FinishDrag()
        {
            var colliders = Physics2D.OverlapPointAll(transform.position);
            foreach (var collider in colliders)
            {
                var worldMapCell = collider.GetComponent<MapWorldCell>();
                if (worldMapCell != null)
                {
                    var cardTarget = new CardTargetData
                    {
                        MapPostion = worldMapCell.MapPosition
                    };

                    if (!_handCardsController.TryPlayCard(_cardIndex, cardTarget))
                    {
                        CancelDrag();
                    }
                    return;
                }
            }
            CancelDrag();
        }

        public void UpdateDrag(Vector2 position)
        {
            transform.position = position;
        }
    }

}
