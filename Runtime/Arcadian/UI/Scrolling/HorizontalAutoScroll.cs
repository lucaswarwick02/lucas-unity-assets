using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Arcadian.UI.Scrolling
{
    public class HorizontalAutoScroll : AbstractAutoScroll
    {
        public override void Select(GameObject selectedGameObject)
        {
            var rectTransform = selectedGameObject.transform as RectTransform;
            if (!rectTransform) return;

            var localPos = rectTransform.localPosition;
            var leftObj = localPos.x - (rectTransform.pivot.x * rectTransform.rect.width);
            var rightObj = localPos.x + ((1 - rectTransform.pivot.x) * rectTransform.rect.width);

            if (layoutGroup)
            {
                leftObj -= layoutGroup.padding.left;
                rightObj += layoutGroup.padding.right;
            }

            var leftView = 0;
            var rightView = leftView + viewport.rect.width;

            var offset = content.localPosition.x - leftView;

            // Right of Rect
            if (rightObj + offset > rightView)
            {
                Debug.Log("A");
                var diff = rightObj + offset - rightView;
                var contentLocalPos = content.localPosition;
                contentLocalPos.x -= diff;

                if (_autoScroll != null) StopCoroutine(_autoScroll);
                _autoScroll = StartCoroutine(AutoScroll(contentLocalPos));
            }

            // Left of Rect
            if (leftObj + offset < leftView)
            {
                Debug.Log("B");
                var diff = leftObj + offset - leftView;
                var contentLocalPos = content.localPosition;
                contentLocalPos.x -= diff;

                if (_autoScroll != null) StopCoroutine(_autoScroll);
                _autoScroll = StartCoroutine(AutoScroll(contentLocalPos));
            }
        }
    }
}