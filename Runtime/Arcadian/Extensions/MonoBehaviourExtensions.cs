using System;
using System.Collections;
using UnityEngine;

namespace Arcadian.Extensions
{
    /// <summary>
    /// A set of <c>MonoBehaviour</c> extension methods for timing and scheduling actions. Useful for invoking callbacks at the end of a frame, or smoothly running logic over a given duration without writing custom coroutines.
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Invoke an action at the end of a frame.
        /// </summary>
        /// <param name="mono">MonoBehaviour script to create the coroutine from.</param>
        /// <param name="action">Action to be invoked.</param>
        public static void InvokeEndOnFrame(this MonoBehaviour mono, Action action)
        {
            if (mono == null || action == null) return;

            mono.StartCoroutine(InvokeEndOfFrameCoroutine(action));
        }

        private static IEnumerator InvokeEndOfFrameCoroutine(Action action)
        {
            yield return new WaitForEndOfFrame();

            action();
        }

        /// <summary>
        /// Run an action over a specified duration, where the elapsed time is passed into the function.
        /// </summary>
        /// <param name="_">MonoBehaviour script</param>
        /// <param name="duration">Length of time to run the function over.</param>
        /// <param name="onProgress">Action to be called each frame.</param>
        /// <param name="useUnscaledTime">If true, use unscaled time (ignores Time.timeScale).</param>
        /// <returns></returns>
        public static IEnumerator RunOverTime(this MonoBehaviour _, float duration, Action<float> onProgress, bool useUnscaledTime = false)
        {
            var timer = 0f;

            while (timer < duration)
            {
                onProgress(timer / duration);

                timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                yield return null;
            }

            onProgress(1f);
        }
    }
}