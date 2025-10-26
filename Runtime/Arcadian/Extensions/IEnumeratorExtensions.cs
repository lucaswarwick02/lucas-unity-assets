using System;
using System.Collections;
using UnityEngine;

namespace Arcadian.Extensions
{
    /// <summary>
    /// Contains extension methods for IEnumerators.
    /// </summary>
    public static class IEnumeratorExtensions
    {
        /// <summary>
        /// Invoke functions before, after, and during a given duration.
        /// Allows for both scaled and unscaled time, and custom animation curves (Lerp, SmoothStep, etc)
        /// </summary>
        /// <param name="duration">Total length to run it for.</param>
        /// <param name="onStart">Invoked at the start.</param>
        /// <param name="onUpdate">Invoked during, with an evaluated percentage.</param>
        /// <param name="onComplete">Invoked at the end.</param>
        /// <param name="curve">Custom interpolation function. Defaults to linear.</param>
        /// <param name="useUnscaledTime">Whether or not to use unscaled time.</param>
        /// <returns></returns>
        public static IEnumerator Tween(
            float duration,
            Action onStart,
            Action<float> onUpdate,
            Action onComplete,
            AnimationCurve curve = null,
            bool useUnscaledTime = false)
        {
            // If not specified, use a linear increase
            curve ??= AnimationCurve.Linear(0f, 0f, 1f, 1f);

            // Start with a unique action
            onStart?.Invoke();

            var timer = 0f;
            while (timer < duration)
            {
                // Update the timer
                timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                // Invoke the update function using the percentage duration, evaluated against the curve
                onUpdate?.Invoke(curve.Evaluate(Mathf.Clamp01(timer / duration)));

                yield return null;
            }

            // One final update with a full duration
            onUpdate?.Invoke(1);

            // Finally invoke the completion action
            onComplete?.Invoke();
        }
    }
}