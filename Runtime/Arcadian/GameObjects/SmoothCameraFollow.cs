using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arcadian.GameObjects
{
    public class SmoothCameraFollow : MonoBehaviour
    {
        private static SmoothCameraFollow _instance;
        
        public Transform target; // The player's transform
        public Vector3 offset; // Offset from the player
        public bool ignoreZ = true;
        
        public float smoothTime = 0.3f;
        public float maxDistance = 10f; // Maximum distance from target
        public float speedMultiplier = 2f; // Speed multiplier when far from target

        private Vector3 _velocity = Vector3.zero;
        private Vector3 _originalPosition;
        private bool _isShaking;

        private void Awake()
        {
            _instance = this;
        }

        private void FixedUpdate()
        {
            // Only move the camera if we aren't shaking
            // if (_isShaking) return;
            
            var targetPosition = target.position + offset;
            var currentPosition = transform.position;

            // Calculate distance to target
            var distance = Vector3.Distance(currentPosition, targetPosition);

            // Adjust smooth time based on distance
            var adjustedSmoothTime = smoothTime;
            if (distance > maxDistance)
            {
                adjustedSmoothTime /= speedMultiplier;
            }

            // Calculate new position
            var newPos = Vector3.SmoothDamp(currentPosition, targetPosition, ref _velocity, adjustedSmoothTime);
            
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

        public static void Shake(ShakeStrength shakeStrength, float duration)
        {
            if (_instance._isShaking) return;

            _instance.StartCoroutine(_instance.ShakeCoroutine(shakeStrength, duration));
        }

        private IEnumerator ShakeCoroutine(ShakeStrength shakeStrength, float duration)
        {
            _isShaking = true;
            _originalPosition = transform.localPosition;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                var x = Random.Range(-1f, 1f) * GetShakeIntensity(shakeStrength);
                var y = Random.Range(-1f, 1f) * GetShakeIntensity(shakeStrength);

                transform.localPosition += new Vector3(x,  y, 0);

                elapsed += Time.deltaTime;

                yield return null;
            }

            _isShaking = false;
        }
        
        private static float GetShakeIntensity(ShakeStrength strength)
        {
            return strength switch
            {
                ShakeStrength.Low => 0.03f,
                ShakeStrength.Medium => 0.06f,
                ShakeStrength.High => 0.09f,
                _ => GetShakeIntensity(ShakeStrength.Low)
            };
        }
    }
    
    public enum ShakeStrength
    {
        Low,
        Medium,
        High
    }
}