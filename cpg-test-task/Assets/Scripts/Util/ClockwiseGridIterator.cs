using System;
using UnityEngine;

namespace Util
{
    public class ClockwiseGridIterator
    {
        public Vector2Int currentPositon;
        public Direction currentDirection;
        public int currentCircle;

        private Vector2Int _lastCirclePos;

        public ClockwiseGridIterator(Vector2Int startingPosition, Direction startingDirection, int startingCircle)
        {
            currentPositon = startingPosition;
            currentDirection = startingDirection;
            currentCircle = startingCircle;

            _lastCirclePos = currentPositon - currentDirection.Forward();
        }

        public Vector2Int Next()
        {
            if (currentPositon == _lastCirclePos)
                StepCircle();
            else
                Step();
            return currentPositon;
        }

        private void StepCircle()
        {
            ++currentCircle;
            currentDirection = Direction.Right;
            currentPositon = new Vector2Int(0, currentCircle);
            _lastCirclePos = currentPositon - currentDirection.Forward();
        }

        private void Step()
        {
            var nextPosition = currentPositon + currentDirection.Forward();
            if (Math.Abs(nextPosition.x) > currentCircle
                || Math.Abs(nextPosition.y) > currentCircle)
            {
                currentDirection = currentDirection.NextClockWise();
                nextPosition = currentPositon + currentDirection.Forward();
            }

            currentPositon = nextPosition;
        }
    }
}