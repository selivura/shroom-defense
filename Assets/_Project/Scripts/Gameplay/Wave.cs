using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [Serializable]
    public class Wave
    {
        [SerializeField] private List<Unit> _unitPrefabs;
        public IReadOnlyList<Unit> UnitPrefabs => _unitPrefabs;
    }


}
