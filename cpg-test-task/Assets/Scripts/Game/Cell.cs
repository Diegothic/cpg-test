using UnityEngine;

namespace Game
{
    public class Cell : MonoBehaviour
    {
        private GridGame _grid;
        private Vector2Int _gridPosition;

        private GridItem _item;
        private bool _isBlocked;

        public void Setup(GridGame grid, Vector2Int newGridPosition, Color color, bool isBlocked)
        {
            _grid = grid;
            _gridPosition = newGridPosition;
            var worldPosition = _grid.GridToWorldPosition(newGridPosition);
            _isBlocked = isBlocked;
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0.1f);

            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
        }

        public bool IsEmpty()
        {
            return !_isBlocked && _item == null;
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

        public void Clear()
        {
            if (_item != null)
                Destroy(_item.gameObject);
            SetItem(null);
        }
    }
}