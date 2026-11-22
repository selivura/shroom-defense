using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class EnemyUnit : Unit
    {
        public void Advance()
        {
            Move(Position + Vector2Int.left);
        }

        public override void ExecuteAction()
        {
            Debug.Log("Enemy unit executed action");
        }
    }
}
