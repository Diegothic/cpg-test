using System;
using Framework;
using UnityEngine;

namespace Controls
{
    public class CameraManager : MonoBehaviour
    {
        public ConfigManager config;

        private Camera _camera;

        private float _zoomLevel;
        private float _zoomMultiplier;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            var gridConfig = config.GetConfig().grid;
            var gridSize = Math.Max(gridConfig.width, gridConfig.height);
            _zoomMultiplier = gridSize / 2.0f;
            _camera.orthographicSize = 5.0f;
        }

        public void SetZoomLevel(float amount)
        {
            _zoomLevel = Math.Clamp(amount, 0.0f, 1.0f);
            _camera.orthographicSize = (_zoomLevel * _zoomMultiplier) + 5.0f;
        }
    }
}