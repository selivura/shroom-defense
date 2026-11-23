using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/DamagePlayerOnAction")]
    public class DamagePlayerOnActionUnitComponent : UnitComponent
    {
        private PlayerController _player;
        [SerializeField] private int _damageAmount = 1;

        [Inject]
        public void Construct(PlayerController player)
        {
            _player = player;
        }

        public override void OnExecuteAction()
        {
            _player.TakeDamage(new Damage(unit, _damageAmount));
            Debug.Log($"{unit.name} deals {_damageAmount} damage TO PLAYER!");
        }
    }
}

