using ShroomDefense.Core;
using UnityEngine;
using VContainer;

namespace ShroomDefense.UI
{
    public class PopupFactory : MonoBehaviour
    {
        [SerializeField] private PopupView _popupViewPrefab;

        private ObjectPool<PopupView> _pool;

        [Inject]
        public void Construct()
        {
            _pool = new(_popupViewPrefab, 10, transform);
        }
        public PopupView Create(string text, Color color, Vector2 position)
        {
            var popup = _pool.GetFreeElement();
            popup.PopUpText = text;
            popup.PopUpColor = color;
            popup.transform.position = position;
            return popup;
        }
    }
}
