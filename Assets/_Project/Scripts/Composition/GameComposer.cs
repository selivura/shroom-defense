using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace ShroomDefense
{
    public class GameComposer : IStartable
    {
        private TurnManager _turnManager;

        public GameComposer(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }
        public void Start()
        {
            UniTask.Void(() => StartGameSequence());
        }

        private async UniTaskVoid StartGameSequence()
        {
            Debug.Log("3");
            await UniTask.WaitForSeconds(1);
            Debug.Log("2");
            await UniTask.WaitForSeconds(1);
            Debug.Log("1");
            await UniTask.WaitForSeconds(1);
            _turnManager.StartGame();
        }
    }
}
