using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arcadian.UI.Scrolling
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class AbstractAutoScroll : MonoBehaviour
    {
        private const float Speed = 10f;
        
        protected LayoutGroup layoutGroup;
        protected RectTransform viewport;
        protected RectTransform content;

        protected Coroutine _autoScroll;

        void Awake()
        {
            var scroll = GetComponent<ScrollRect>();

            viewport = scroll.viewport;
            content = scroll.content;

            layoutGroup = content.GetComponent<LayoutGroup>();

            if (!viewport || !content)
            {
                Debug.LogError($"[Arcadian] {name}: ScrollRect must have viewport and content assigned.");
            }
        }

        protected IEnumerator AutoScroll(Vector3 targetLocalPos)
        {
            while (Vector3.Distance(content.localPosition, targetLocalPos) > 0.1f)
            {
                content.localPosition =
                    Vector3.Lerp(content.localPosition, targetLocalPos, Time.unscaledDeltaTime * Speed);

                yield return null;
            }

            content.localPosition = targetLocalPos;
            _autoScroll = null;
        }

        /// <summary>
        /// Once a <c>Selectable</c> is selected, this function is run.
        /// The Horizontal/Vertical Auto Scroll components use this to run the AutoScroll function.
        /// </summary>
        /// <param name="selectedGameObject">Object which is selected.</param>
        public abstract void Select(GameObject selectedGameObject);
    }
}