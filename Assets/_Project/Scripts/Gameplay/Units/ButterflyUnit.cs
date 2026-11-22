using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    public class ButterflyUnit : EnemyUnit
    {
        [SerializeField] private Unit _caterpillarPrefab;

        private UnitFactory _factory;

        [Inject]
        public void SetupFactory(UnitFactory factory)
        {
            _factory = factory;
        }

        public override void ExecuteAction()
        {
            Vector2Int? emptyCell = FindEmptyEnemyCell();
            if (emptyCell.HasValue && _caterpillarPrefab != null)
            {
                var caterpillar = _factory.Create(_caterpillarPrefab);
                _battleMap.PlaceUnitAt(emptyCell.Value, caterpillar);
                Debug.Log("Бабочка призвала Гусеницу!");
            }
            else
            {
                Debug.Log("Бабочка хотела призвать гусеницу, но поле врага забито!");
            }
        }

        private Vector2Int? FindEmptyEnemyCell()
        {
            for (int y = 0; y < _battleMap.Rows.Count; y++)
            {
                for (int x = 1; x <= 3; x++)
                {
                    if (!_battleMap.Rows[y].Units.ContainsKey(x))
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return null; 
        }
    }
}