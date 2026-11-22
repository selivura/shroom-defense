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

        public Vector2Int MinBounds = new(-5, 0);
        public Vector2Int MaxBounds = new(5, 2);

        [SerializeField] private MapWorldCell _mapWorldCellPrefab;

        private void Start()
        {
            int boundSizeX = MaxBounds.x - MinBounds.x + 1;
            int boundSizeY = MaxBounds.y - MinBounds.y + 1;
            for (int x = 0; x < boundSizeX; x++)
            {
                var posX = MinBounds.x + x;
                if(posX == 0)
                {
                    continue;
                }
                for (int y = 0; y < boundSizeY; y++)
                {
                    var posY = MinBounds.y + y;
                    var cell = Instantiate(_mapWorldCellPrefab, new Vector3(posX * UnitWorldDistance.x, posY * UnitWorldDistance.y, 0), Quaternion.identity, transform);
                    cell.MapPosition = new Vector2Int(posX, posY);
                }
            }
        }

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
