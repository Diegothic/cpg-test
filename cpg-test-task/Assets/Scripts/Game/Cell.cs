using UnityEngine;

namespace Game
{
    public class Cell : MonoBehaviour
    {
        private GameGrid _grid;

        private Vector2 _worldPosition;

        private bool _isBlocked;

        public void Setup(GameGrid grid, Vector2Int newGridPosition, Color color, bool isBlocked)
        {
            _grid = grid;
            _worldPosition = _grid.GetWorldPosition(newGridPosition);
            _isBlocked = isBlocked;
            transform.position = new Vector3(_worldPosition.x + 0.5f, _worldPosition.y + 0.5f, 0.0f);

            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
        }

        public bool IsEmpty()
        {
            return !_isBlocked;
        }
    }
}