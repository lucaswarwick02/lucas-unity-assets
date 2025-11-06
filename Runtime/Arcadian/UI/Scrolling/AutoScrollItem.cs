using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Arcadian.UI.Scrolling
{
    /// <summary>
    /// Automatically assigned to <c>Selectable</c> children to invoke the scroll animations.
    /// </summary>
    [AddComponentMenu("")]  // Hide from the 'Add Component' Menu
    internal class AutoScrollItem : MonoBehaviour, ISelectHandler
    {
        internal AbstractAutoScroll autoScroll;
        
        /// <summary>
        /// When this item is selected, tell the AutoScroll to move.
        /// </summary>
        public void OnSelect(BaseEventData _)
        {
            autoScroll.Select(gameObject);
        }
    }
}