using UnityEngine;

namespace Game
{
    public abstract class GridItem : MonoBehaviour
    {
        private GridGame _grid;
        private Vector2Int _gridPosition;

        public virtual void Setup(GridGame grid, Vector2Int gridPosition)
        {
            _grid = grid;
            _gridPosition = gridPosition;
        }

        public GridGame GetGrid()
        {
            return _grid;
        }

        public Vector2Int GetGridPosition()
        {
            return _gridPosition;
        }

        public abstract bool IsAdjacent();
    }
}