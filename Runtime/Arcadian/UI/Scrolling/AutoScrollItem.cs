using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Arcadian.UI.Scrolling
{
    public class AutoScrollItem : MonoBehaviour, ISelectHandler
    {
        public AbstractAutoScroll autoScroll;
        
        public void OnSelect(BaseEventData eventData)
        {
            autoScroll.Select(gameObject);
        }
    }
}