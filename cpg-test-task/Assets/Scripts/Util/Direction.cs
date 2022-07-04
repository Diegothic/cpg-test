using UnityEngine;

namespace Util
{
    public class Direction
    {
        public static readonly Direction Up = new Direction(new Vector2Int(0, 1));
        public static readonly Direction Right = new Direction(new Vector2Int(1, 0));
        public static readonly Direction Down = new Direction(new Vector2Int(0, -1));
        public static readonly Direction Left = new Direction(new Vector2Int(-1, 0));

        private readonly Vector2Int _forward;

        private Direction(Vector2Int forward)
        {
            _forward = forward;
        }

        public Vector2Int Forward()
        {
            return _forward;
        }

        public Direction NextClockWise()
        {
            if (_forward == Up._forward)
                return Right;
            if (_forward == Right._forward)
                return Down;
            if (_forward == Down._forward)
                return Left;
            return Up;
        }
    }
}