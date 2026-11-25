using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

namespace ShroomDefense.UI
{
    public class PopupView : MonoBehaviour
    {
        private PanelRenderer _panelRenderer;
        [CreateProperty] public string PopUpText = "?";
        [CreateProperty] public StyleColor PopUpColor = Color.white;

        [SerializeField] private float _moveY = 1;

        private void Awake()
        {
            _panelRenderer = GetComponentInChildren<PanelRenderer>();
            if (_panelRenderer == null)
            {
                throw new System.NullReferenceException("PanelRenderer not found in UnitHUDView or its children.");
            }
        }

        private void OnEnable()
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReolad);
            DOTween.Sequence(transform)
                .Append(transform.DOMoveY(transform.position.y + _moveY, 0.5f))
                .AppendCallback(() => gameObject.SetActive(false));
            
        }
        private void OnDisable()
        {
            _panelRenderer.UnregisterUIReloadCallback(OnUIReolad);
            transform.DOKill();
        }

        private void OnUIReolad(PanelRenderer panelRenderer, VisualElement rootElement, int version)
        {
            var popupLabel = rootElement.Q<Label>("PopupLabel");
            popupLabel.SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(PopupView.PopUpText))
            });
            popupLabel.SetBinding("style.color", new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(PopupView.PopUpColor))
            });
            popupLabel.dataSource = this;
        }
    }
}
