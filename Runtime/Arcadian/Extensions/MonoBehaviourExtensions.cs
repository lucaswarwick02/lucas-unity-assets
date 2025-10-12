using System;
using System.Collections;
using UnityEngine;

namespace Arcadian.Extensions
{
    public static class MonoBehaviourExtensions
    {
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