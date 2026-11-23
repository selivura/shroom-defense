using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [CreateAssetMenu(fileName = "StatType", menuName = "Scriptable Objects/StatType")]
    public class StatType : ScriptableObject
    {
        [field: SerializeField]
        public string DisplayName { get; private set; }
        [field: SerializeField]
        public string Description { get; private set; }
    }
}
