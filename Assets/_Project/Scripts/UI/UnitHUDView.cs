using NaughtyAttributes;
using ShroomDefense.Gameplay;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShroomDefense.UI
{
    public class UnitHUDView : MonoBehaviour
    {
        [SerializeField][Required]private UnitViewModel _unitVM;
        private PanelRenderer _panelRenderer;
        private void Awake()
        {
            _panelRenderer = GetComponentInChildren<PanelRenderer>();
            if(_panelRenderer == null)
            {
                throw new System.NullReferenceException("PanelRenderer not found in UnitHUDView or its children.");
            }
        }

        private void OnEnable()
        {
            _panelRenderer.RegisterUIReloadCallback(OnUIReolad);
        }
        private void OnDisable()
        {
            _panelRenderer.UnregisterUIReloadCallback(OnUIReolad);
        }

        private void OnUIReolad(PanelRenderer panelRenderer, VisualElement rootElement, int version)
        {
            var healthLabel = rootElement.Q<Label>("Health");
            healthLabel.SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(UnitViewModel.Health))
            });
            healthLabel.dataSource = _unitVM;

            var damageLabel = rootElement.Q<Label>("Damage");
            damageLabel.SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(UnitViewModel.Damage))
            });
            damageLabel.dataSource = _unitVM;
        }
    }
}
