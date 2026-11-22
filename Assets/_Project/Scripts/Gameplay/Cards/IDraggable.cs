using UnityEngine;

namespace ShroomDefense.Gameplay
{
    public interface IDraggable
    { 
        public void BeginDrag();
        public void UpdateDrag(Vector2 position);
        public void CancelDrag();
        public void FinishDrag();
    }

}
