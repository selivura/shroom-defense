using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class HeavyPrivateUnit : Unit
    {
        private void Start()
        {
            AddShield();
        }
        public override void ExecuteAction()
        {
            Debug.Log("HeavyPrivateUnit unit executed action"); 
        }
    }
}

