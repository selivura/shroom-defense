using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class Shovel : MonoBehaviour
    {
        private BattleMap _battleMap;
        private CardTargetData _pos1;
        private CardTargetData _pos2;


        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }

        public void SetPos1(CardTargetData pos1)
        {
            _pos1 = pos1;
        }

        public void SetPos2(CardTargetData pos2)
        {
            _pos2 = pos2;
        }
        public void Dig()
        {
            _battleMap.Rows[_pos1.MapPostion.y].Units[_pos1.MapPostion.x].Move(_pos2.MapPostion);
            _pos1 = null;
            _pos2 = null;
        }
    }
}
