using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    public enum AmplifyType
    {
        PercentScaleAdd = 0,
        PercentScaleMulti = 1,
        Multiplier = 2,
    }
    public enum ScaleType
    {
        Percentage = 0,
        Multiplier = 1,
    }

    [System.Serializable]
    public class StatController
    {
        [SerializeField] public Dictionary<StatType, Stat> Stats = new Dictionary<StatType, Stat>();

        public bool GetStatValue(StatType statType, out float statValue)
        {
            statValue = 0;
            if (GetStat(statType, out var stat))
            {
                statValue = stat.Value;
                return true;
            }
            return false;
        }
        public bool GetStat(StatType statType, out Stat stat)
        {
            if (Stats.TryGetValue(statType, out stat))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns stat scaled by value. Returns 0 if stat does not exist.
        /// </summary>
        /// <param name="statType">Stat that will be scaled</param>
        /// <param name="scale">Scale value</param>
        /// <param name="scaleType">Type of scale</param>
        /// <returns></returns>
        public float ScaleStat(StatType statType, float scale, ScaleType scaleType)
        {
            if(GetStatValue(statType, out var statValue))
            {
                switch (scaleType)
                {
                    case ScaleType.Percentage:
                        return statValue * (scale / 100);
                        
                    case ScaleType.Multiplier:
                        return statValue * scale;
                       
                    default:
                        return 0;
                        
                }
            }
            return 0;
        }

        public void AddModifierToAllStatsOfTypes(List<StatType> statTypes, StatModifier modifier)
        {
            foreach (var statType in statTypes)
            {
                if (GetStat(statType, out var stat))
                {
                    stat.AddModifier(modifier);
                }
            }
        }
    }
}
