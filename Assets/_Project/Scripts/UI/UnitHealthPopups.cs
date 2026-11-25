using NaughtyAttributes;
using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;

namespace ShroomDefense.UI
{
    public class UnitHealthPopups : MonoBehaviour
    {
        [SerializeField] [Required] private Unit _unit;
        [SerializeField] private string _damagePrefix = "-";
        [SerializeField] private string _healPrefix = "+";
        [SerializeField] private string _damageAbsorbedText = "Absrobed";
        [SerializeField] private Color _damageAbsorbedColor = Color.white;
        [SerializeField] private Color _damageTakenColor = Color.red;
        [SerializeField] private Color _healColor = Color.green;
        private PopupFactory _popupFactory;


        [Inject]
        public void Construct(PopupFactory popupFactory)
        {
            _popupFactory = popupFactory;
        }

        private void OnEnable()
        {
            _unit.OnTakeDamage.AddListener(OnDamage);
            _unit.OnTakeHeal.AddListener(OnHeal);
            _unit.OnDamageAbsorbed.AddListener(OnAbsorb);
        }

        private void OnDisable()
        {
            _unit.OnTakeDamage.RemoveListener(OnDamage);
            _unit.OnTakeHeal.RemoveListener(OnHeal);
            _unit.OnDamageAbsorbed.RemoveListener(OnAbsorb);
        }

        private void OnAbsorb()
        {
            _popupFactory.Create(_damageAbsorbedText, _damageAbsorbedColor, transform.position);
        }

        private void OnHeal(int arg0)
        {
            _popupFactory.Create(_healPrefix + arg0, _healColor, transform.position);
        }

        private void OnDamage(int arg0)
        {
            _popupFactory.Create(_damagePrefix + arg0, _damageTakenColor, transform.position);
        }
    }
}
