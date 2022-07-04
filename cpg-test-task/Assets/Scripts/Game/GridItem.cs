using UnityEngine;

namespace Game
{
    public class GridItem : MonoBehaviour
    {
        private GridGame _grid;
        private Vector2Int _gridPosition;
        private Cell _cell;

        public virtual void Setup(GridGame grid, Vector2Int gridPosition)
        {
            _grid = grid;
            _gridPosition = gridPosition;
            _cell = _grid.GetCell(gridPosition);
        }

        public GridGame GetGrid()
        {
            return _grid;
        }

        public Vector2Int GetGridPosition()
        {
            return _gridPosition;
        }

        public Cell GetCell()
        {
            return _cell;
        }
    }
}