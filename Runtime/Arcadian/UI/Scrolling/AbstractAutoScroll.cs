using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arcadian.UI.Scrolling
{
    public abstract class AbstractAutoScroll : MonoBehaviour
    {
        private const float Speed = 10f;
        
        [SerializeField] protected LayoutGroup layoutGroup;
        [SerializeField] protected RectTransform viewport;
        [SerializeField] protected RectTransform content;
        
        protected Coroutine _autoScroll;
        
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

        public abstract void Select(GameObject selectedGameObject);
    }
}