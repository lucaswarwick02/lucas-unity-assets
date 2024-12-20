using UnityEngine;

namespace Arcadian.GameObjects
{
    public class SmoothCameraFollow : MonoBehaviour
    {
        public Transform target; // The player's transform
        public Vector3 offset; // Offset from the player
        public bool ignoreZ = true;
        
        public float smoothTime = 0.3f;
        public float maxDistance = 10f; // Maximum distance from target
        public float speedMultiplier = 2f; // Speed multiplier when far from target

        private Vector3 _velocity = Vector3.zero;

        private void FixedUpdate()
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 currentPosition = transform.position;

            // Calculate distance to target
            float distance = Vector3.Distance(currentPosition, targetPosition);

            // Adjust smooth time based on distance
            float adjustedSmoothTime = smoothTime;
            if (distance > maxDistance)
            {
                adjustedSmoothTime /= speedMultiplier;
            }

            // Calculate new position
            Vector3 newPos = Vector3.SmoothDamp(currentPosition, targetPosition, ref _velocity, adjustedSmoothTime);
            
            // Clamp to max distance if needed
            if (distance > maxDistance)
            {
                newPos = Vector3.ClampMagnitude(newPos - targetPosition, maxDistance) + targetPosition;
            }

            // Ignore Z if specified
            if (ignoreZ)
            {
                newPos.z = currentPosition.z;
            }

            transform.position = newPos;
        }
    }
}