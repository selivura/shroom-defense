using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [CreateAssetMenu(fileName = "CardsDatabase", menuName = "Scriptable Objects/CardsDatabase")]
    public class CardsDatabase : ScriptableObject
    {
        [SerializeField] private Card[] _cards;
        public IReadOnlyList<Card> Cards => _cards;
    }
}
