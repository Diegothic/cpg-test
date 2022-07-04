using System;
using UnityEngine;

namespace Util
{
    public class ClockwiseGridIterator
    {
        public Vector2Int CurrentPosition;
        public int CurrentCircle;

        private Direction _currentDirection;
        private Vector2Int _lastCirclePos;

        public ClockwiseGridIterator(Vector2Int startingPosition, Direction startingDirection, int startingCircle)
        {
            CurrentPosition = startingPosition;
            _currentDirection = startingDirection;
            CurrentCircle = startingCircle;

            _lastCirclePos = CurrentPosition - _currentDirection.Forward();
        }

        public void Next()
        {
            if (CurrentPosition == _lastCirclePos)
                StepCircle();
            else
                Step();
        }

        private void StepCircle()
        {
            ++CurrentCircle;
            _currentDirection = Direction.Right;
            CurrentPosition = new Vector2Int(0, CurrentCircle);
            _lastCirclePos = CurrentPosition - _currentDirection.Forward();
        }

        private void Step()
        {
            var nextPosition = CurrentPosition + _currentDirection.Forward();
            if (Math.Abs(nextPosition.x) > CurrentCircle
                || Math.Abs(nextPosition.y) > CurrentCircle)
            {
                _currentDirection = _currentDirection.NextClockWise();
                nextPosition = CurrentPosition + _currentDirection.Forward();
            }

            CurrentPosition = nextPosition;
        }
    }
}