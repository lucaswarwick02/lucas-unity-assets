using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Arcadian.UI.Scrolling
{
    /// <summary>
    /// A component extending AbstractAutoScroll that automatically scrolls horizontally to keep a selected UI element within view. It adjusts the content position based on the selected element’s bounds relative to the viewport and uses smooth coroutine-based motion for transitions.
    /// </summary>
    public class HorizontalAutoScroll : AbstractAutoScroll
    {
        /// <summary>
        /// Once a <c>Selectable</c> is selected, this function is run.
        /// Used to run the AutoScroll function.
        /// </summary>
        /// <param name="selectedGameObject">Object which is selected.</param>
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
                var diff = rightObj + offset - rightView;
                var contentLocalPos = content.localPosition;
                contentLocalPos.x -= diff;

                if (_autoScroll != null) StopCoroutine(_autoScroll);
                _autoScroll = StartCoroutine(AutoScroll(contentLocalPos));
            }

            // Left of Rect
            if (leftObj + offset < leftView)
            {
                var diff = leftObj + offset - leftView;
                var contentLocalPos = content.localPosition;
                contentLocalPos.x -= diff;

                if (_autoScroll != null) StopCoroutine(_autoScroll);
                _autoScroll = StartCoroutine(AutoScroll(contentLocalPos));
            }
        }
    }
}