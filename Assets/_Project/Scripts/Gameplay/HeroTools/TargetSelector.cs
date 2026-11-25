using ShroomDefense.Gameplay;
using ShroomDefense.Input;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace ShroomDefense
{
    public class TargetSelector : MonoBehaviour
    {
        private InputService _inputService;

        bool _isSelecting = false;

        public UnityEvent<CardTargetData> OnTargetPicked;
        private Vector2 _worldPointerPosition => Camera.main.ScreenToWorldPoint(_inputService.PlayerActionsInput.PointerPosition);

        [Inject]
        public void Construct(InputService inputService)
        {
            _inputService = inputService;
        }

        private void OnEnable()
        {
            _inputService.PlayerActionsInput.OnPointerClickEvent += OnMouseClick;
        }

        private void OnDisable()
        {
            _inputService.PlayerActionsInput.OnPointerClickEvent -= OnMouseClick;
        }

        public void StartSelecting()
        {
            _isSelecting = true;
            gameObject.SetActive(true);
            Debug.Log("Started selecting");
        }

        private void Update()
        {
            transform.position = _worldPointerPosition;
        }

        private void OnMouseClick()
        {
            if (!_isSelecting) return;

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

                    _isSelecting = false;
                    gameObject.SetActive(false);
                    OnTargetPicked.Invoke(cardTarget);
                    Debug.Log("Click success");
                    return;
                }
            }
            Debug.Log("Click bad");
            CancelSelecting();
        }

        public void CancelSelecting()
        {
            _isSelecting = false;
            gameObject.SetActive(false);
            OnTargetPicked.Invoke(null);
            Debug.Log("Cancelled selection");
        }
    }
}
