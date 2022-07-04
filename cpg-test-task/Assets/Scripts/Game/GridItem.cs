using UnityEngine;

namespace Game
{
    public class GridItem : MonoBehaviour
    {
        private GameGrid _grid;
        private Vector2Int _gridPosition;
        private Cell _cell;

        public virtual void Setup(GameGrid grid, Vector2Int gridPosition)
        {
            _grid = grid;
            _gridPosition = gridPosition;
            _cell = _grid.GetCell(gridPosition);
        }

        public GameGrid GetGrid()
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