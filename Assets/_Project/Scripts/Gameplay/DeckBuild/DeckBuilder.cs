using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class DeckBuilder : MonoBehaviour
    {
        [SerializeField] [Required] private CardsDatabase _database;
        private PersistentDeck _persistentDeck;

        [Inject]
        public void Construct(PersistentDeck persistentDeck)
        {
            _persistentDeck = persistentDeck;
        }

        public void DraftCardsDummy()
        {
            // TODO: Add actual card drafting
            _persistentDeck.DraftedDeck.CardPrefabs.AddRange(_database.Cards);
        }
    }
}
