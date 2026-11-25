using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [Serializable]
    public class Wave
    {
        [SerializeField] private List<Unit> _unitPrefabs;
        [field: SerializeField] public int WaveDuration { get; private set; } = 3;
        public IReadOnlyList<Unit> UnitPrefabs => _unitPrefabs;
    }
}
