using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [CreateAssetMenu(fileName = "UnitFaction", menuName = "Scriptable Objects/UnitFaction")]
    public class UnitFaction : ScriptableObject
    {
        [field:SerializeField] public string Name { get; private set; }
        [field:TextArea(3, 10)]
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}
