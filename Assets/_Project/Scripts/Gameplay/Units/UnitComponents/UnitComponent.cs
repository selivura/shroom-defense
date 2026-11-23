using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [RequireComponent(typeof(UnitComponent))]
    public abstract class UnitComponent : MonoBehaviour
    {
        protected Unit unit { get; private set; }

        /// <summary>
        /// Используется <see cref="Unit"/> для инициализации компонента
        /// </summary>
        /// <param name="unit"></param>
        public void InitializeComponent(Unit unit)
        {
            this.unit = unit;
            OnComponentInit();
        }

        /// <summary>
        /// Вызывается сразу после инициализации компонента, ДО инжекта в компонент зависимостей. см. <see cref="Unit.Construct"/>
        /// </summary>
        protected virtual void OnComponentInit()
        {

        }

        public virtual void OnExecuteAction()
        {

        }
    }
}
