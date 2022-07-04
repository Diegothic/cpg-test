using System;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controls
{
    public class CameraManager : MonoBehaviour
    {
        public ConfigManager config;

        private Camera _camera;

        [SerializeField]
        private float baseZoom = 5.0f;
        private float _zoomLevel;
        private float _zoomMultiplier;

        private Vector2 _lastMousePos;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            var gridConfig = config.GetConfig().grid;
            var gridSize = Math.Max(gridConfig.width, gridConfig.height);
            _zoomMultiplier = gridSize / 2.0f;
            _camera.orthographicSize = baseZoom;

            _lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !IsMouseOverUI())
                PanView();
            _lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void PanView()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mouseDelta = mousePos - _lastMousePos;
            transform.position = new Vector3(
                transform.position.x - mouseDelta.x,
                transform.position.y - mouseDelta.y,
                transform.position.z
            );
            _lastMousePos = mousePos;
        }

        public void SetZoomLevel(float amount)
        {
            _zoomLevel = Math.Clamp(amount, 0.0f, 1.0f);
            _camera.orthographicSize = (_zoomLevel * _zoomMultiplier) + baseZoom;
        }
    }
}