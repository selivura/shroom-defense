using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/DamagePlayer")]
    public class DamagePlayerUnitComponent : UnitComponent
    {
        private PlayerController _player;
        [SerializeField] private int _damageAmount = 1;

        [Inject]
        public void Construct(PlayerController player)
        {
            _player = player;
        }

        public override async UniTask DoComponentAction()
        {
            _player.TakeDamage(new Damage(unit, _damageAmount));
            Debug.Log($"{unit.name} deals {_damageAmount} damage TO PLAYER!");
        }
    }
}

