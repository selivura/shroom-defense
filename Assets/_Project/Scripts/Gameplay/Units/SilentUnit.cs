using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class SilentUnit : Unit, ITaunt
    {
        public bool IsTaunt => true;
        private void Start()
        {
            AddShield();
        }
        public override void ExecuteAction()
        {
            Debug.Log("SilentUnit unit executed action"); 
        }
    }
}

