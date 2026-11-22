using ShroomDefense.Input;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class PlayerDragNDropController : MonoBehaviour
    {
        private InputService _inputService;

        private IDraggable _currentDraggable;
        private Vector2 _worldPointerPosition => Camera.main.ScreenToWorldPoint(_inputService.PlayerActionsInput.PointerPosition);

        [Inject]
        public void Construct(InputService inputService)
        {
            _inputService = inputService;
        }

        private void OnEnable()
        {
            _inputService.PlayerActionsInput.OnPointerPressedEvent += OnPointerPressed;
            _inputService.PlayerActionsInput.OnPointerReleaseEvent += OnPointerReleased;
        }

        private void OnDisable()
        {
            _inputService.PlayerActionsInput.OnPointerPressedEvent -= OnPointerPressed;
            _inputService.PlayerActionsInput.OnPointerReleaseEvent -= OnPointerReleased;
        }

        private void Update()
        {
            if(_currentDraggable != null)
            {
                _currentDraggable.UpdateDrag(_worldPointerPosition);
            }
        }

        private void OnPointerPressed()
        {
            var colliders = Physics2D.OverlapPointAll(_worldPointerPosition);

            foreach (var collider in colliders)
            {
                var draggable = collider.GetComponent<IDraggable>();
                if (draggable != null)
                {
                    _currentDraggable = draggable;
                    draggable.BeginDrag();
                    break;
                }
            }
        }

        private void OnPointerReleased()
        {
            if (_currentDraggable != null)
            {
                _currentDraggable.UpdateDrag(_worldPointerPosition);
                _currentDraggable.FinishDrag();
                _currentDraggable = null;
            }
        }
    }
}
