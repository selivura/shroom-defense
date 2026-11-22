using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense
{
    public class UnitFactory : MonoBehaviour
    {
        private IObjectResolver _resolver;
        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        public T Create<T>(T prefab) where T : Unit
        {
            return _resolver.Instantiate(prefab);
        }
    }
}
