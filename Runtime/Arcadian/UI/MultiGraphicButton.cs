using UnityEngine;
using UnityEngine.UI;

namespace Arcadian.UI
{
    public class MultiGraphicButton : Button
    {
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            // Get all graphics to tint
            var graphics = GetComponentsInChildren<Graphic>();

            // Return early if there aren't any graphics
            if (graphics == null || graphics.Length == 0) return;
 
            var targetColor = state switch { 
                SelectionState.Disabled => colors.disabledColor,
                SelectionState.Highlighted => colors.highlightedColor,
                SelectionState.Normal => colors.normalColor,
                SelectionState.Pressed => colors.pressedColor,
                SelectionState.Selected => colors.selectedColor,
                _ => Color.white 
            };
 
            foreach (var graphic in graphics)
            {
                graphic.CrossFadeColor(targetColor, instant ? 0 : colors.fadeDuration, true, true);
            }
        }
    }
}