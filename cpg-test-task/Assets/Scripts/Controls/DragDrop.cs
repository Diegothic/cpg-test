using System;
using Game;
using UnityEngine;

namespace Controls
{
    public class DragDrop : MonoBehaviour
    {
        [SerializeField]
        private string colliderTag;

        private bool _dragging;
        private bool _usingTouch;

        private GridItem _gridItem;

        private Vector3 _currentWorldPos;

        private void Start()
        {
            _gridItem = GetComponent<GridItem>();
        }

        private void Update()
        {
            CheckForDragStart();
            CheckForDragEnd();

            if (_dragging)
                FollowDragPosition();
        }

        private void FollowDragPosition()
        {
            _currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(_currentWorldPos.x, _currentWorldPos.y, transform.position.z);
        }

        private void CheckForDragStart()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    _dragging = CheckDragRaycast(touch.position);
                    _usingTouch = true;
                }
            }

            if (!_usingTouch && Input.GetMouseButtonDown(1))
            {
                _dragging = CheckDragRaycast(Input.mousePosition);
            }
        }

        private void CheckForDragEnd()
        {
            if ((!_usingTouch && Input.GetMouseButtonUp(1) && _dragging)
                || (_usingTouch && Input.touchCount == 0))
            {
                _usingTouch = false;
                _dragging = false;
                PlaceItem();
            }
        }

        private void PlaceItem()
        {
            var grid = _gridItem.GetGrid();
            grid.CellAt(_gridItem.GetGridPosition()).SetItem(null);
            var gridPosition = grid.WorldToGridPosition(_currentWorldPos);
            gridPosition.x = Math.Clamp(gridPosition.x, 0, grid.GetWidth() - 1);
            gridPosition.y = Math.Clamp(gridPosition.y, 0, grid.GetHeight() - 1);
            var nearestEmpty = grid.FindNearestEmptyCell(gridPosition);
            grid.ResetIterator();
            grid.CellAt(nearestEmpty).SetItem(_gridItem);
        }

        private bool CheckDragRaycast(Vector2 screenPosition)
        {
            var rayPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            var rayDirection = Camera.main.transform.forward;
            var hit = Physics2D.Raycast(rayPosition, rayDirection);
            return hit.collider != null && hit.collider.CompareTag(colliderTag);
        }
    }
}