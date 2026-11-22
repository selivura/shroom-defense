using Cysharp.Threading.Tasks;
using ShroomDefense.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense
{
    public class DeckbuildComposer : IStartable
    {
        private DeckBuilder _deckBuilder;

        public DeckbuildComposer(DeckBuilder deckBuilder)
        {
            _deckBuilder = deckBuilder;
        }

        public void Start()
        {
            _deckBuilder.DraftCardsDummy();
            UniTask.Create(() => LoadIntoGameplay());
        }

        private async UniTask LoadIntoGameplay()
        {
            await SceneManager.UnloadSceneAsync("Deckbuild").ToUniTask();
            await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive).ToUniTask();
        }
    }
}
