using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [Serializable]
    public class Deck
    {
        public List<Card> CardPrefabs = new();

        public Deck(List<Card> cards)
        {
            CardPrefabs = new List<Card>(cards);
        }

        public Deck()
        {

        }
    }
}
