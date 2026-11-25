using UnityEngine;
using UnityEngine.Events;

namespace ShroomDefense.Gameplay
{
    public class HeroTool : MonoBehaviour
    {
        [field: SerializeField] public int ManaCost { get; private set; } = 2;

        public UnityEvent OnToolStartUse;
        public UnityEvent OnSuccessfullyUsed;
        public UnityEvent OnCancelledUse;

        private bool _isToolInUse;

        public void UseTool()
        {
            OnToolStartUse.Invoke();
        }

        public bool CanUseTool()
        {
            return !_isToolInUse;
        }

        public void SucceedUse()
        {
            _isToolInUse = false;
            OnSuccessfullyUsed.Invoke();
        }

        public void CancelUse()
        {
            _isToolInUse = false;
            OnCancelledUse.Invoke();
        }
    }
}
