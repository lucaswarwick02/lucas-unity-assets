using UnityEngine;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// A lightweight static helper providing reusable Unity <c>AnimationCurve</c> presets for common "ease in" and "ease out" transitions. Useful for animations, UI effects, or smooth value interpolation without manually defining curves each time.
    /// </summary>
    public static class Curves
    {
        /// <summary>
        /// AnimationCurve that starts at 0 and ends at 1.
        /// </summary>
        public static readonly AnimationCurve In = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        /// <summary>
        /// AnimationCurve that starts at 1 and ends at 0.
        /// </summary>
        public static readonly AnimationCurve Out = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        
        /// <summary>
        /// Normal-distribution-like bump, peak at 0.5.
        /// </summary>
        public static readonly AnimationCurve Bell = new(
            new Keyframe(0f, 0f),
            new Keyframe(0.25f, 0.6f),
            new Keyframe(0.5f, 1f),
            new Keyframe(0.75f, 0.6f),
            new Keyframe(1f, 0f)
        );
    }
}