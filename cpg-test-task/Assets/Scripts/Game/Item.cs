using UnityEngine;
using Util;

namespace Game
{
    public class Item : GridItem
    {
        [SerializeField]
        private ItemType type;

        [SerializeField]
        private float lerpSpeed = 10.0f;

        private bool _animating = true;
        private Vector2 _desiredPosition;

        private bool _isAdjacent;

        public override void Setup(GridGame grid, Vector2Int gridPosition)
        {
            base.Setup(grid, gridPosition);
            _desiredPosition = GetGrid().GridToWorldPosition(GetGridPosition());
            GetComponent<SpriteRenderer>().color = type.color;

            _isAdjacent = false;
            CheckNeighbour(Direction.Up);
            CheckNeighbour(Direction.Right);
            CheckNeighbour(Direction.Down);
            CheckNeighbour(Direction.Left);
        }

        private void CheckNeighbour(Direction direction)
        {
            var checkedPosition = GetGridPosition() + direction.Forward();
            if (!GetGrid().IsInBounds(checkedPosition))
                return;

            var neighbour = GetGrid().CellAt(checkedPosition).GetItem() as Item;
            if (neighbour != null && IsTheSameType(neighbour))
            {
                _isAdjacent = true;
                neighbour._isAdjacent = true;
            }
        }

        public void Update()
        {
            if (_animating)
                Animate();
        }

        private void Animate()
        {
            Vector2 newPosition = GetAnimatedPosition();
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }

        public override bool IsAdjacent()
        {
            return _isAdjacent;
        }

        private Vector2 GetAnimatedPosition()
        {
            if (Vector2.Distance(transform.position, _desiredPosition) > float.Epsilon)
                return Vector2.Lerp(transform.position, _desiredPosition, Time.deltaTime * lerpSpeed);
            _animating = false;
            return _desiredPosition;
        }

        private bool IsTheSameType(Item other)
        {
            return type == other.type;
        }
    }
}