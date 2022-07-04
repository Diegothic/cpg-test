using UnityEngine;

namespace Game
{
    public class Cell : MonoBehaviour
    {
        private GameGrid _grid;

        private Vector2 _worldPosition;

        public void Setup(GameGrid grid, Vector2Int newGridPosition, Color color)
        {
            _grid = grid;
            _worldPosition = _grid.GetWorldPosition(newGridPosition);
            transform.position = new Vector3(_worldPosition.x + 0.5f, _worldPosition.y + 0.5f, 0.0f);

            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
        }
    }
}