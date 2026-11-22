using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class MushroomGuardUnit: Unit, ITaunt
    {
        public bool IsTaunt => true;

        public override void ExecuteAction()
        {
            Debug.Log("MushroomGuardUnit executed action"); 
        }
    }
}
