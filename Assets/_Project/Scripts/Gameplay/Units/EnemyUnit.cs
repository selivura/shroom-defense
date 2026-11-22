using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class EnemyUnit : Unit
    {
        public void Advance()
        {
            var movePosition = Position + Vector2Int.left;
            if (_battleMap.ValidatePosition(movePosition))
            {
                Move(movePosition);
            }
        }

        public override void ExecuteAction()
        {
            Debug.Log("Enemy unit executed action");
        }
    }
}
