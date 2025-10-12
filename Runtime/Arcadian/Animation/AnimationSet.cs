using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Arcadian.Animation
{
    /// <summary>
    /// A reusable Unity component for managing multiple animtaions on a <c>SpriteRenderer</c> or <c>UI.Image</c>. Useful for switching between named animation states (like character animations, UI transisitons, or effects) with lightweight, scriptable control over frame timing, looping, and automatic frame updates.
    /// </summary>
    public class AnimationSet : MonoBehaviour
    {
        public Component target;

        public string defaultAnimation;
        public AnimationState[] animationStates;
        public float frameDelay = 0.125f;

        public bool playOnStart = true;
        public bool useUnscaledTime;

        public event Action onFrameChange;
        
        private bool _isPlaying;
        private float _timer;
        
        public AnimationState CurrentAnimation { get; private set; }
        public int CurrentFrame { get; private set; }
        
        public Component Target
        {
            get => target;
            set
            {
                if (value is SpriteRenderer || value is Image || value == null)
                {
                    target = value;
                }
            }
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(defaultAnimation)) SetAnimation(defaultAnimation);

            if (playOnStart) StartAnimation();
        }

        /// <summary>
        /// Change to a new animation. Does nothing if already active.
        /// </summary>
        /// <param name="animationName">Name of the animation in the list of animation states.</param>
        /// <param name="ignoreNameMatch">Whether or not to ignore the check for if the names are the same</param>
        public void SetAnimation(string animationName, bool ignoreNameMatch = false)
        {
            if (!ignoreNameMatch && CurrentAnimation.name == animationName) return; // Animation already playing

            if (!animationStates.Select(state => state.name).Contains(animationName))
            {
                Debug.LogWarning("Attempted to set animation to " + animationName + " but it does not exist in this animation set.");
                return;
            }

            CurrentAnimation = animationStates.First(state => state.name == animationName);
            CurrentFrame = 0;
        }
        
        /// <summary>
        /// Invokes the NextFrame function.
        /// </summary>
        public void StartAnimation()
        {
            _isPlaying = true;
            NextFrame();
        }

        /// <summary>
        /// Cancels the reoccurring NextFrame function.
        /// </summary>
        public void StopAnimation()
        {
            _isPlaying = false;
        }

        private void Update()
        {
            if (!_isPlaying) return;
            
            _timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (_timer < frameDelay) return;

            _timer = 0;
            NextFrame();
        }

        private void NextFrame()
        {
            if (string.IsNullOrEmpty(CurrentAnimation.name)) return;

            CurrentFrame++;
            if (CurrentFrame >= CurrentAnimation.sprites.Length) CurrentFrame = 0;

            if (target is SpriteRenderer sp) sp.sprite = CurrentAnimation.sprites[CurrentFrame];
            if (target is Image img) img.sprite = CurrentAnimation.sprites[CurrentFrame];
            
            onFrameChange?.Invoke();
        }
    }

    [Serializable]
    public struct AnimationState
    {
        public string name;
        public Sprite[] sprites;
    }
}