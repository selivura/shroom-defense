using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public abstract class Card : MonoBehaviour
    {
        protected BattleMap battleMap;

        [Inject]
        public void Construct(BattleMap battleMap)
        {
            this.battleMap = battleMap;
        }
        public abstract void ExecuteAction(CardTargetData target);
    }

    public class CardTargetData
    {
        public Vector2Int MapPostion;
    }
}
