using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense.Composition
{
    public class CoreComposer : IStartable
    {
        public void Start()
        {
            UniTask.Create(() => SceneManager.LoadSceneAsync("Deckbuild", LoadSceneMode.Additive).ToUniTask());
        }
    }
}
