using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense.Gameplay
{
    public class CardFactory : MonoBehaviour
    {
        private IObjectResolver _resolver;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        public Card Create(Card prefab)
        {
            return _resolver.Instantiate(prefab);
        }
    }
}
