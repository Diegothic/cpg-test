using UnityEngine;

namespace Game
{
    public class Item : GridItem
    {
        [SerializeField]
        private ItemType type;

        [SerializeField]
        private float lerpSpeed = 10.0f;

        private Vector2 _desiredPosition;

        public override void Setup(GameGrid grid, Vector2Int gridPosition)
        {
            base.Setup(grid, gridPosition);
            _desiredPosition = GetGrid().GetWorldPosition(GetGridPosition());
            GetComponent<SpriteRenderer>().color = type.color;
        }

        public void Update()
        {
            Vector2 newPosition = GetAnimatedPosition();
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }

        private Vector2 GetAnimatedPosition()
        {
            if (Vector2.Distance(transform.position, _desiredPosition) > float.Epsilon)
                return Vector2.Lerp(transform.position, _desiredPosition, Time.deltaTime * lerpSpeed);
            return _desiredPosition;
        }
    }
}