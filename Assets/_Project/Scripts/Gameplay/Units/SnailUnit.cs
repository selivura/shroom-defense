using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class SnailUnit : EnemyUnit
    {
        public override void ExecuteAction()
        {
            Heal(3);
            Debug.Log("Улитка восстановила 3 здоровья!");
        }
    }
}