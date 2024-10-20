using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AoJCabViewer {

    public class CameraMouseController : MonoBehaviour {

        [SerializeField] private float _speed = 1f;
        [SerializeField] private Vector3 _moveMin;
        [SerializeField] private Vector3 _moveMax;
        [SerializeField] private float _translationSensitivity = 0.05f;
        [SerializeField] private float _zoomSensitivity = 3f;

        [SerializeField] private float _rotationSensitivity = 1f;
        private bool _clickedOverUI;

        void Update() {
            ValidateMouse();
        }

        /// <summary>
        /// Calls UpdateCamera() if the user is clicking on the scene view (not the UI or outside of the app
        /// window), or if the user moves over the UI or out of the window after clicking inside of it.
        /// </summary>
        private void ValidateMouse() {
            // If the mouse is not over UI or out of window, we can update camera position.
            if (!MCP.Instance.UIManager.IsMouseOverUIOrOutOfWindow() && !_clickedOverUI) {
                UpdateCamera();
            } else {

                // If no buttons are being clicked, set _clickedOverUI to false.
                if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)) {
                    _clickedOverUI = false;
                    return;
                }

                // If we are over the UI or out of the window, set _clickedOverUI to true.
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
                    _clickedOverUI = true;
                    return;
                }

                if ((Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)) && !_clickedOverUI) {
                    UpdateCamera();
                    return;
                }

            }
        }

        /// <summary>
        /// Provides Unity Scene View-like camera camera controls.
        /// </summary>
        private void UpdateCamera() {
            float translateX = 0;
            float translateY = 0;

            if (Input.GetMouseButton(0)) {
                translateY = Input.GetAxis("Mouse Y") * _translationSensitivity;
                translateX = Input.GetAxis("Mouse X") * _translationSensitivity;
            }

            float zoom = Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity;
            transform.Translate(new Vector3(-translateX, -translateY, zoom) * Time.deltaTime * _speed);

            // Validate Position
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Clamp(newPosition.x, _moveMin.x, _moveMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, _moveMin.y, _moveMax.y);
            newPosition.z = Mathf.Clamp(newPosition.z, _moveMin.z, _moveMax.z);
            transform.position = newPosition;

            float rotationX = 0;
            float rotationY = 0;

            if (Input.GetMouseButton(1)) {
                rotationX = Input.GetAxis("Mouse Y") * _rotationSensitivity;
                rotationY = Input.GetAxis("Mouse X") * _rotationSensitivity;
            }

            transform.Rotate(0, rotationY, 0, Space.World);
            transform.Rotate(-rotationX, 0, 0);
        }

    }
}