using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [CreateAssetMenu(fileName = "HeroToolsDatabase", menuName = "Scriptable Objects/HeroToolsDatabase")]
    public class HeroToolsDatabase : ScriptableObject
    {
        [SerializeField] private HeroTool[] _tools;
        public IReadOnlyList<HeroTool> Tools => _tools;
    }
}
