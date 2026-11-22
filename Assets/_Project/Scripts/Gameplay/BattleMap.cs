using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    public class BattleMap : MonoBehaviour
    {
        /// <summary>
        /// Each row has columns. Negative index columns are player cells, positive - enemy.
        /// </summary>
        public List<Row> Rows = new() { new(), new(), new() };
        public Vector2Int UnitWorldDistance = new(1, 1);

        public void PlaceUnitAt(Vector2Int coordinates, Unit unit)
        {
            if(!ValidatePosition(coordinates))
            {
                throw new InvalidOperationException($"Coordinates {coordinates} are not in scope of the current map");
            }
            var row = Rows[coordinates.y];
            if (row.Units.ContainsKey(coordinates.x))
            {
                Debug.LogWarning("Tried to place unit in cell where another unit already sits");
                return;
            }
            row.Units[coordinates.x] = unit;

            unit.transform.position = new Vector2(coordinates.x * UnitWorldDistance.x, coordinates.y * UnitWorldDistance.y);
        }

        public bool ClearCell(Vector2Int position)
        {
            return Rows[position.y].Units.Remove(position.x);
        }

        public bool ValidatePosition(Vector2Int coordinates)
        {
            if (coordinates.x == 0)
            {
                return false;
            }
            if (coordinates.y < 0 || coordinates.y >= Rows.Count)
            {
                return false;
            }
            return true;
        }
    }

    [Serializable]
    public class Row
    {
        [SerializeField] public Dictionary<int, Unit> Units = new();
    }
}
