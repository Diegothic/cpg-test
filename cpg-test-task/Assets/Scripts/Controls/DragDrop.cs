using System;
using Game;
using UnityEngine;

namespace Controls
{
    public class DragDrop : MonoBehaviour
    {
        private bool _dragging;
        private GridItem _gridItem;

        private Vector3 _currentWorldPos;

        private void Start()
        {
            _gridItem = GetComponent<GridItem>();
        }

        private void OnMouseDown()
        {
            _dragging = true;
        }

        private void OnMouseUp()
        {
            _dragging = false;
            PlaceItem();
        }

        private void PlaceItem()
        {
            var grid = _gridItem.GetGrid();
            grid.GetCell(_gridItem.GetGridPosition()).SetItem(null);
            var gridPosition = grid.GetGridPosition(_currentWorldPos);
            gridPosition.x = Math.Clamp(gridPosition.x, 0, grid.GetWidth() - 1);
            gridPosition.y = Math.Clamp(gridPosition.y, 0, grid.GetHeight() - 1);
            var nearestEmpty = grid.FindNearestEmptyCell(gridPosition);
            grid.GetCell(nearestEmpty).SetItem(_gridItem);
        }

        private void Update()
        {
            if (_dragging)
            {
                _currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(_currentWorldPos.x, _currentWorldPos.y, transform.position.z);
            }
        }
    }
}