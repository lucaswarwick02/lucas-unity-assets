using System;
using System.Collections;
using UnityEngine;

namespace Arcadian.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static void RunEndOnFrame(this MonoBehaviour monoBehaviour, Action function)
        {
            monoBehaviour.StartCoroutine(DelayedFunction(function));
        }

        private static IEnumerator DelayedFunction(Action function)
        {
            yield return new WaitForEndOfFrame();
            
            function?.Invoke();
        }

        public static IEnumerator Progress(this MonoBehaviour monoBehaviour, float duration, Action<float> function)
        {
            var timer = 0f;

            while (timer < duration)
            {
                function(timer / duration);
                
                timer += Time.deltaTime;
                
                yield return null;
            }
            
            function(1f);
        }
    }
}