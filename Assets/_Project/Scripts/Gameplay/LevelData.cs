using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
    public class LevelData : ScriptableObject
    {
        [field:SerializeField] public int RowsNumber { get; private set; } = 3;
        [field: SerializeField] public int FurthestEnemyColumn { get; private set; } = 3;
        [field: SerializeField] public int FurthestPlayerColumn { get; private set; } = -3;
        [SerializeField] private List<Wave> _waves;
        public IReadOnlyList<Wave> Waves => _waves;
    }
}
