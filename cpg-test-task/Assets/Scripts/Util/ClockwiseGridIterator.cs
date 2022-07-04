using System;
using UnityEngine;

namespace Util
{
    public class ClockwiseGridIterator
    {
        private Vector2Int _currentPosition;
        private Direction _currentDirection;
        private int _currentCircle;
        private Vector2Int _lastCirclePos;

        public ClockwiseGridIterator(Vector2Int startingPosition, Direction startingDirection, int startingCircle)
        {
            _currentPosition = startingPosition;
            _currentDirection = startingDirection;
            _currentCircle = startingCircle;

            _lastCirclePos = _currentPosition - _currentDirection.Forward();
        }

        public void Next()
        {
            if (_currentPosition == _lastCirclePos)
                StepCircle();
            else
                Step();
        }

        public Vector2Int GetCurrentPosition()
        {
            return _currentPosition;
        }

        public int GetCurrentCircle()
        {
            return _currentCircle;
        }

        private void StepCircle()
        {
            ++_currentCircle;
            _currentDirection = Direction.Right;
            _currentPosition = new Vector2Int(0, _currentCircle);
            _lastCirclePos = _currentPosition - _currentDirection.Forward();
        }

        private void Step()
        {
            var nextPosition = _currentPosition + _currentDirection.Forward();
            if (Math.Abs(nextPosition.x) > _currentCircle
                || Math.Abs(nextPosition.y) > _currentCircle)
            {
                _currentDirection = _currentDirection.NextClockwise();
                nextPosition = _currentPosition + _currentDirection.Forward();
            }

            _currentPosition = nextPosition;
        }
    }
}