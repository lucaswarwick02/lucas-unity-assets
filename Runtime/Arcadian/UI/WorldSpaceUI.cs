using UnityEngine;

namespace Arcadian.UI
{
    public class WorldSpaceUI : MonoBehaviour
    {
        public Vector3 WorldPosition { get; set; }

        private static Camera _camera;

        void Awake()
        {
            if (!_camera) _camera = Camera.main;
        }

        void LateUpdate()
        {
            if (!_camera) return;

            var screenPos = _camera.WorldToScreenPoint(WorldPosition);
            if (screenPos.z <= 0f) return;  // Avoid flipping when behind the camera

            transform.position = screenPos;
        }
    }
}