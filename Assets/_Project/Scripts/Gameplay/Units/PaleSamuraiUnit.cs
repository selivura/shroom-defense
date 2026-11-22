using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class PaleSamuraiUnit : Unit
    {
        public override void ExecuteAction()
        {
            Debug.Log("PaleSamuraiUnit executed action");
            TakeDamage(new Damage(this, 2));
        }
    }
}

