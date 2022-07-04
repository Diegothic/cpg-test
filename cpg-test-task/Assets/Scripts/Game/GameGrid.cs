using Framework;
using UnityEngine;

namespace Game
{
    public class GameGrid : MonoBehaviour
    {
        [SerializeField]
        private GameObject cellPrefab;

        [SerializeField]
        private Color evenCellColor;
        [SerializeField]
        private Color oddCellColor;
        [SerializeField]
        private Color blockedCellColor;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float blockedChance = 0.25f;

        private int _width;
        private int _height;

        private Vector2 _startingCellPosition;

        private Cell[,] _cells;

        private void Start()
        {
            var config = GetComponent<ConfigManager>().GetConfig();
            _width = config.grid.width;
            _height = config.grid.height;

            _startingCellPosition.x = -(_width / 2.0f);
            _startingCellPosition.y = -(_height / 2.0f);

            _cells = new Cell[_width, _height];

            SetupCells();
        }

        public Vector2 GetWorldPosition(Vector2Int gridPosition)
        {
            return _startingCellPosition + gridPosition;
        }

        public Vector2Int GetGridPosition(Vector2 worldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPosition.x + _width / 2.0f),
                Mathf.FloorToInt(worldPosition.y + _height / 2.0f)
            );
        }

        public bool IsInBounds(Vector2Int gridPosition)
        {
            return gridPosition.x >= 0
                   && gridPosition.y >= 0
                   && gridPosition.x < _width
                   && gridPosition.y < _height;
        }

        private void SetupCells()
        {
            var gridPosition = new Vector2Int();
            for (var y = 0; y < _height; ++y)
            {
                for (var x = 0; x < _width; ++x)
                {
                    var createdCell = Instantiate(cellPrefab);
                    gridPosition.Set(x, y);
                    _cells[x, y] = createdCell.GetComponent<Cell>();
                    var blocked = ShouldBeBlocked();
                    var color = CreateCellColor(gridPosition, blocked);
                    _cells[x, y].Setup(this, gridPosition, color, blocked);
                }
            }
        }

        private bool ShouldBeBlocked()
        {
            return Random.Range(float.Epsilon, 1.0f) <= blockedChance;
        }

        private Color CreateCellColor(Vector2Int gridPosition, bool blocked)
        {
            if (blocked)
                return blockedCellColor;
            return IsEvenCell(gridPosition) ? evenCellColor : oddCellColor;
        }

        private bool IsEvenCell(Vector2Int gridPosition)
        {
            return gridPosition.y % 2 == 0 ? gridPosition.x % 2 == 0 : gridPosition.x % 2 != 0;
        }
    }
}