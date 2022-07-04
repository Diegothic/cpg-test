﻿using System;
using Framework;
using UnityEngine;

namespace Controls
{
    public class CameraManager : MonoBehaviour
    {
        public ConfigManager config;

        private Camera _camera;

        private float _baseZoom = 5.0f;
        private float _zoomLevel;
        private float _zoomMultiplier;

        private Vector2 lastMousePos;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            var gridConfig = config.GetConfig().grid;
            var gridSize = Math.Max(gridConfig.width, gridConfig.height);
            _zoomMultiplier = gridSize / 2.0f;
            _camera.orthographicSize = _baseZoom;

            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var mouseDelta = mousePos - lastMousePos;
                transform.position = new Vector3(
                    transform.position.x - mouseDelta.x,
                    transform.position.y - mouseDelta.y,
                    transform.position.z
                );
            }

            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void SetZoomLevel(float amount)
        {
            _zoomLevel = Math.Clamp(amount, 0.0f, 1.0f);
            _camera.orthographicSize = (_zoomLevel * _zoomMultiplier) + _baseZoom;
        }
    }
}