using ShroomDefense.Gameplay;
using Unity.Properties;
using UnityEngine;

namespace ShroomDefense.UI
{
    public class UnitViewModel : MonoBehaviour
    {
        [SerializeField] Unit _unit;
        [SerializeField] private StatType _damageStatType;

        [CreateProperty] public int Health => _unit.CurrentHP;
        private Stat _unitDamageStat = null;

        [CreateProperty]
        public int Damage
        {
            get
            {
                if(_unitDamageStat == null)
                {
                    if(_unit.StatController.GetStat(_damageStatType, out _unitDamageStat))
                    {
                        return 0;
                    }
                }
                return Mathf.RoundToInt(_unitDamageStat.Value);
            }
        }
    }
}
