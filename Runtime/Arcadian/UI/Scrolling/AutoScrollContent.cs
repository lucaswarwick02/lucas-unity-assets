using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Arcadian.UI.Scrolling
{
    /// <summary>
    /// A component that automatically attaches <c>AutoScrollItem</c> components to all <c>Selectable</c> UI elements under its hierarchy, linking them to the nearest parent <c>AbstractAutoScroll</c>. It ensures that new children added at runtime are also registered for automatic scrolling behaviour.
    /// </summary>
    public class AutoScrollContent : MonoBehaviour
    {
        private AbstractAutoScroll autoScroll;

        void Awake()
        {
            autoScroll = GetComponentInParent<AbstractAutoScroll>();
            if (!autoScroll)
            {
                Debug.Log($"[Arcadian] {name}: Must be under a ScrollRect with AbstractAutoScroll");
            }
        }

        private void Start()
        {
            OnTransformChildrenChanged();
        }

        private void OnTransformChildrenChanged()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                
                if (child.GetComponent<Selectable>() == null) continue;

                if (child.GetComponent<AutoScrollItem>()) continue;

                var autoScrollItem = child.gameObject.AddComponent<AutoScrollItem>();
                autoScrollItem.autoScroll = autoScroll;
            }
        }
    }
}