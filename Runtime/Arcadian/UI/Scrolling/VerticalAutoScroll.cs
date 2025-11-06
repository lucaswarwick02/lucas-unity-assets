using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arcadian.UI.Scrolling
{
    /// <summary>
    /// A Unity component extending AbstractAutoScroll that automatically scrolls vertically to keep a selected UI element within view. It adjusts the content position based on the selected element’s bounds relative to the viewport and uses smooth coroutine-based motion for transitions.
    /// </summary>
    public class VerticalAutoScroll : AbstractAutoScroll
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
            var topObj = localPos.y + ((1 - rectTransform.pivot.y) * rectTransform.rect.height);
            var bottomObj = localPos.y - (rectTransform.pivot.y * rectTransform.rect.height);

            if (layoutGroup)
            {
                topObj += layoutGroup.padding.top;
                bottomObj -= layoutGroup.padding.bottom;
            }

            var topView = 0;
            var bottomView = topView - viewport.rect.height;

            var offset = topView - content.localPosition.y;

            // Below rect
            if (bottomObj - offset < bottomView)
            {
                var diff = -(bottomObj - offset - bottomView);
                var contentLocalPos = content.localPosition;
                contentLocalPos.y += diff;
                
                if (_autoScroll != null) StopCoroutine(_autoScroll);
                _autoScroll = StartCoroutine(AutoScroll(contentLocalPos));
            }

            // Above Rect
            if (topObj - offset > topView)
            {
                var diff = -(topObj - offset - topView);
                
                var contentLocalPos = content.localPosition;
                contentLocalPos.y += diff;
                
                if (_autoScroll != null) StopCoroutine(_autoScroll);
                _autoScroll = StartCoroutine(AutoScroll(contentLocalPos));
            }
        }
    }
}