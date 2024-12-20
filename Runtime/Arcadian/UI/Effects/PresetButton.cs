using Arcadian.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arcadian.UI.Effects
{
    public class PresetButton : FunctionalButton
    {
        [SerializeField] private TMP_Text[] texts;
        [SerializeField] private Material defaultPreset;
        [SerializeField] private Material selectedPreset;
        
        public override void OnButtonHover()
        {
            texts.ForEach(text => text.fontSharedMaterial = selectedPreset);
        }

        public override void OnButtonHoverEnd()
        {
            texts.ForEach(text => text.fontSharedMaterial = defaultPreset);
        }

        public override void OnButtonPress(PointerEventData eventData = null)
        {
            //
        }
    }
}