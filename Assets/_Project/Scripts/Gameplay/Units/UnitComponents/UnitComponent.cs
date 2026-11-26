using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    public enum ActionExecuteType
    {
        Never,
        OnExecuteAction,
        OnInitialize,
    }
    [RequireComponent(typeof(Unit))]
    public abstract class UnitComponent : MonoBehaviour
    {
        protected Unit unit { get; private set; }
        public float DelayBeforeAction = 0;
        public float DelayAfterAction = 0.1f;
        [SerializeField] private ActionExecuteType _autoExecuteActionOn = ActionExecuteType.Never;

        /// <summary>
        /// Используется <see cref="Unit"/> для инициализации компонента
        /// </summary>
        /// <param name="unit"></param>
        public async UniTaskVoid InitializeComponent(Unit unit)
        {
            this.unit = unit;
            await OnComponentInit();
        }

        /// <summary>
        /// Вызывается после инжекта зависимостей и инициализации компонента. см. <see cref="Unit.Construct"/>
        /// </summary>
        protected async UniTask OnComponentInit()
        {
            if (_autoExecuteActionOn == ActionExecuteType.OnInitialize)
            {
                await DoComponentAction();
            }
        }

        public async UniTask ExecuteAction()
        {
            if (_autoExecuteActionOn == ActionExecuteType.OnExecuteAction)
            {
                await DoComponentAction();
            }
        }

        public virtual async UniTask DoComponentAction()
        {
            // Перенести реализацию сюда
        }
    }
}
