using System;
using Framework;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameGrid : MonoBehaviour
    {
        [SerializeField]
        private float cellSize = 1.0f;

        [SerializeField]
        private GameObject cellPrefab;
        [SerializeField]
        private GameObject spawnerPrefab;

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
            SetupSpawner();
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public Vector2 GetWorldPosition(Vector2Int gridPosition)
        {
            return _startingCellPosition + gridPosition + new Vector2(cellSize / 2.0f, cellSize / 2.0f);
        }

        public Vector2Int GetGridPosition(Vector2 worldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPosition.x + _width / 2.0f),
                Mathf.FloorToInt(worldPosition.y + _height / 2.0f)
            );
        }

        public Cell GetCell(Vector2Int gridPosition)
        {
            return _cells[gridPosition.x, gridPosition.y];
        }

        public Vector2Int FindNearestEmptyCell(Vector2Int gridPosition)
        {
            if (IsInBounds(gridPosition) && GetCell(gridPosition).IsEmpty())
                return gridPosition;

            var lookupDir = Direction.Right;
            var lookupPos = gridPosition + new Vector2Int(0, 1);
            var circle = 1;
            var lastCirclePos = lookupPos - lookupDir.GetForward();
            while (circle < Math.Max(_width, _height))
            {
                if (IsInBounds(lookupPos) && GetCell(lookupPos).IsEmpty())
                    return lookupPos;
                if (lookupPos == lastCirclePos)
                {
                    ++circle;
                    lookupDir = Direction.Right;
                    lookupPos = gridPosition + new Vector2Int(0, circle);
                    lastCirclePos = lookupPos - lookupDir.GetForward();
                }
                else
                {
                    var nextPos = lookupPos + lookupDir.GetForward();
                    if (nextPos.x > gridPosition.x + circle
                        || nextPos.x < gridPosition.x - circle
                        || nextPos.y > gridPosition.y + circle
                        || nextPos.y < gridPosition.y - circle)
                    {
                        lookupDir = lookupDir.GetNextClockWise();
                        nextPos = lookupPos + lookupDir.GetForward();
                    }

                    lookupPos = nextPos;
                }
            }

            return gridPosition;
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
            var centerCell = GetGridPosition(Vector2.zero);
            var gridPosition = new Vector2Int();
            for (var y = 0; y < _height; ++y)
            {
                for (var x = 0; x < _width; ++x)
                {
                    var createdCell = Instantiate(cellPrefab);
                    gridPosition.Set(x, y);
                    _cells[x, y] = createdCell.GetComponent<Cell>();
                    var blocked = gridPosition != centerCell && ShouldBeBlocked();
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

        private void SetupSpawner()
        {
            var spawner = Instantiate(spawnerPrefab);
            GetCell(GetGridPosition(Vector2.zero)).SetItem(spawner.GetComponent<Spawner>());
        }
    }
}