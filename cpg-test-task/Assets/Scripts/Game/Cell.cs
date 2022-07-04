using UnityEngine;

namespace Game
{
    public class Cell : MonoBehaviour
    {
        private GridGame _grid;

        private Vector2Int _gridPosition;
        private Vector2 _worldPosition;

        private GridItem _item;
        private bool _isBlocked;

        public void Setup(GridGame grid, Vector2Int newGridPosition, Color color, bool isBlocked)
        {
            _grid = grid;
            _gridPosition = newGridPosition;
            _worldPosition = _grid.GetWorldPosition(newGridPosition);
            _isBlocked = isBlocked;
            transform.position = new Vector3(_worldPosition.x, _worldPosition.y, 0.1f);

            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
        }

        public bool IsEmpty()
        {
            return !_isBlocked && _item == null;
        }

        public Vector2 GetWorldPosition()
        {
            return _worldPosition;
        }

        public GridItem GetItem()
        {
            return _item;
        }

        public void SetItem(GridItem item)
        {
            _item = item;
            if (_item != null)
                _item.Setup(_grid, _gridPosition);
        }
    }
}