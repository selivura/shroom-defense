using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class FoulWandererUnit : Unit
    {
        public override void ExecuteAction()
        {
            Debug.Log("Mushroom unit executed action"); 
        }

        private void Start()
        {
            // Вызываем метод добора из класса колоды
        }
    }
}

