using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public abstract class Card : MonoBehaviour
    {
        protected BattleMap battleMap;
        public Sprite CardIcon;
        [field: SerializeField] public int ManaCost { get; private set; } = 1;

        [Inject]
        public void Construct(BattleMap battleMap)
        {
            this.battleMap = battleMap;
        }
        public abstract void ExecuteAction(CardTargetData target);
        public virtual int GetCurrentCost()
        {
            return ManaCost;
        }
    }

    public class CardTargetData
    {
        public Vector2Int MapPostion;
    }
}
