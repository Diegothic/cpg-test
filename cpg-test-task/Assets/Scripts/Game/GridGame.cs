using System;
using Framework;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Game
{
    public class GridGame : MonoBehaviour
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

        private Spawner _spawner;
        private bool _spawning;

        private ClockwiseGridIterator _iterator;

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
            ResetIterator();
        }

        private void Update()
        {
            if (_spawning)
                _spawner.SpawnItem();
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public Vector2 GridToWorldPosition(Vector2Int gridPosition)
        {
            return _startingCellPosition + gridPosition + new Vector2(cellSize / 2.0f, cellSize / 2.0f);
        }

        public Vector2Int WorldToGridPosition(Vector2 worldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPosition.x + _width / 2.0f),
                Mathf.FloorToInt(worldPosition.y + _height / 2.0f)
            );
        }

        public Cell CellAt(Vector2Int gridPosition)
        {
            return _cells[gridPosition.x, gridPosition.y];
        }

        public void StartSpawning()
        {
            _spawning = true;
        }

        public void StopSpawning()
        {
            _spawning = false;
            ResetIterator();
        }

        public void ClearAdjacent()
        {
            foreach (var cell in _cells)
            {
                var item = cell.GetItem();
                if (item != null && item.IsAdjacent())
                    cell.Clear();
            }
        }

        public void ResetIterator()
        {
            _iterator = new ClockwiseGridIterator(new Vector2Int(0, 1), Direction.Right, 1);
        }

        public Vector2Int FindNearestEmptyCell(Vector2Int gridPosition)
        {
            if (IsInBounds(gridPosition) && CellAt(gridPosition).IsEmpty())
                return gridPosition;

            while (_iterator.GetCurrentCircle() < Math.Max(_width, _height))
            {
                var lookupPos = _iterator.GetCurrentPosition() + gridPosition;
                if (IsInBounds(lookupPos) && CellAt(lookupPos).IsEmpty())
                    return lookupPos;
                _iterator.Next();
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
            var centerCellPosition = WorldToGridPosition(Vector2.zero);
            var gridPosition = new Vector2Int();
            for (var y = 0; y < _height; ++y)
            {
                for (var x = 0; x < _width; ++x)
                {
                    gridPosition.Set(x, y);
                    CreateCell(gridPosition);
                    var isCenterCell = gridPosition == centerCellPosition;
                    SetupCell(gridPosition, isCenterCell);
                }
            }
        }

        private void CreateCell(Vector2Int gridPosition)
        {
            var createdCell = Instantiate(cellPrefab);
            _cells[gridPosition.x, gridPosition.y] = createdCell.GetComponent<Cell>();
        }

        private void SetupCell(Vector2Int gridPosition, bool isCenterCell)
        {
            var blocked = !isCenterCell && ShouldBeBlocked();
            var color = CreateCellColor(gridPosition, blocked);
            _cells[gridPosition.x, gridPosition.y].Setup(this, gridPosition, color, blocked);
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
            _spawner = spawner.GetComponent<Spawner>();
            CellAt(WorldToGridPosition(Vector2.zero)).SetItem(_spawner);
        }
    }
}