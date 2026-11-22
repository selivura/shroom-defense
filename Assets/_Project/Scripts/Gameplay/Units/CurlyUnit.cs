using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;

namespace ShroomDefense
{
    public class CurlyUnit : Unit
    {
        private void Start()
        {
            var allies = _battleMap.Rows
                .SelectMany(row => row.Units.Values)
                .Where(unit => unit != this && unit.Position.x < 0 && unit.CurrentHP > 0)
                .ToList();
            if (allies.Count == 0)
            {
                return;
            }
            int maxAttack = allies.Max(unit => unit.MaxAttack);
            int maxHP = allies.Max(unit => unit.MaxHP);
            BuffAttack(maxAttack - CurrentAttack);
            BuffMaxHP(maxHP - MaxHP);     
        }
        public override void ExecuteAction()
        {
            Debug.Log("CurlyUnit executed action"); 
        }
    }
}

