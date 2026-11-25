using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class DeckBuilder : MonoBehaviour
    {
        [SerializeField] [Required] private CardsDatabase _database;
        [SerializeField] [Required] private HeroToolsDatabase _heroToolsDB;
        private DeckbuildResults _persistentDeck;

        [Inject]
        public void Construct(DeckbuildResults persistentDeck)
        {
            _persistentDeck = persistentDeck;
        }

        public void DraftCardsDummy()
        {
            // TODO: Add actual card drafting
            // TODO: Add actual hero tool selection
            _persistentDeck.HeroTool = _heroToolsDB.Tools[0];
            _persistentDeck.DraftedDeck.CardPrefabs.AddRange(_database.Cards);
        }
    }
}
