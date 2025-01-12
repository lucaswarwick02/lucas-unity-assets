using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Arcadian.UI.Scrolling
{
    public class AutoScrollContent : MonoBehaviour
    {
        [SerializeField] private AbstractAutoScroll autoScroll;

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