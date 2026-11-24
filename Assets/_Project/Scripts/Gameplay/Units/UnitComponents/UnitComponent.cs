using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [RequireComponent(typeof(Unit))]
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
        /// Вызывается после инжекта зависимостей и инициализации компонента. см. <see cref="Unit.Construct"/>
        /// </summary>
        protected virtual void OnComponentInit()
        {

        }

        public virtual async UniTask OnExecuteAction()
        {

        }
    }
}
