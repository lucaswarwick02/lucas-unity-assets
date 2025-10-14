using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Arcadian.Animation
{
    /// <summary>
    /// A reusable Unity component for managing multiple animations on a <c>SpriteRenderer</c> or <c>UI.Image</c>. Useful for switching between named animation states (like character animations, UI transitions, or effects) with lightweight, scriptable control over frame timing, looping, and automatic frame updates.
    /// </summary>
    public class AnimationGroup : MonoBehaviour
    {
        /// <summary>
        /// The target component to animate. Must be a <c>SpriteRenderer</c> or <c>UI.Image</c>.
        /// </summary>
        public Component target;

        /// <summary>
        /// Name of the animation to use on start.
        /// </summary>
        public string defaultAnimation;

        /// <summary>
        /// List of different animations to pick from.
        /// </summary>
        public AnimationState[] animationStates;

        /// <summary>
        /// Time delay (in seconds) between frames.
        /// </summary>
        public float frameDelay = 0.125f;

        /// <summary>
        /// If true, the animation will automatically play on start.
        /// </summary>
        public bool playOnStart = true;

        /// <summary>
        /// If true, the animation will use unscaled time (ignores Time.timeScale).
        /// </summary>
        public bool useUnscaledTime;

        /// <summary>
        /// Event triggered when the animation frame changes.
        /// </summary>
        public event Action OnFrameChange;

        /// <summary>
        /// Event triggered when the animation finishes a cycle.
        /// </summary>
        public event Action OnAnimationFinished;
        
        private bool _isPlaying;
        private float _timer;
        
        /// <summary>
        /// The current animation that is running.
        /// </summary>
        public AnimationState CurrentAnimation { get; private set; }

        /// <summary>
        /// The current frame that the animation is on. 
        /// </summary>
        public int CurrentFrame { get; private set; }
        
        /// <summary>
        /// Gets or sets the target component to animate. Only allows <c>SpriteRenderer</c> or <c>UI.Image</c>.
        /// </summary>
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
            if (!string.IsNullOrEmpty(defaultAnimation)) Set(defaultAnimation);

            if (playOnStart) Play();
        }

        /// <summary>
        /// Change to a new animation. Does nothing if already active.
        /// </summary>
        /// <param name="animationName">Name of the animation in the list of animation states.</param>
        /// <param name="ignoreNameMatch">Whether or not to ignore the check for if the names are the same</param>
        public void Set(string animationName, bool ignoreNameMatch = false)
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
        /// Starts the animation.
        /// </summary>
        public void Play()
        {
            _isPlaying = true;
            NextFrame();
        }

        /// <summary>
        /// Stops the animation from running. 
        /// </summary>
        public void Stop()
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
            if (CurrentFrame >= CurrentAnimation.sprites.Length)
            {
                CurrentFrame = 0;
                OnAnimationFinished?.Invoke();
            }

            if (target is SpriteRenderer sp) sp.sprite = CurrentAnimation.sprites[CurrentFrame];
            if (target is Image img) img.sprite = CurrentAnimation.sprites[CurrentFrame];
            
            OnFrameChange?.Invoke();
        }
    }

    /// <summary>
    /// Contains the information used to define a single animation loop.
    /// </summary>
    [Serializable]
    public struct AnimationState
    {
        /// <summary>
        /// Name of the animation. Used for setting.
        /// </summary>
        public string name;

        /// <summary>
        /// The sequence of sprites to animate.
        /// </summary>
        public Sprite[] sprites;
    }
}