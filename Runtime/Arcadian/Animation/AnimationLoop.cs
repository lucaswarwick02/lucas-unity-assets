using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Arcadian.Animation
{
    /// <summary>
    /// A reusable Unity component for animating a sequence of sprites on a <c>SpriteRenderer</c> or <c>UI.Image</c>.
    /// Supports looping or one-off animations without using an Animator Controller. 
    /// Allows control over timing, looping, and destruction on completion.
    /// </summary>
    [AddComponentMenu("Arcadian/Animation/Animation Loop")]
    public class AnimationLoop : MonoBehaviour
    {
        [ShowIf("useSpriteRenderer"), BoxGroup("Visuals")]
        public SpriteRenderer spriteRenderer;
        
        [ShowIf("useImage"), BoxGroup("Visuals")]
        public Image image;

        /// <summary>
        /// The sequence of sprites to animate.
        /// </summary>
        [Tooltip("The sequence of sprites to animate."), BoxGroup("Visuals")]
        public Sprite[] sprites;

        /// <summary>
        /// Time delay in seconds between frames.
        /// </summary>
        [Tooltip("Time delay in seconds between frames."), BoxGroup("Settings")]
        public float frameDelay = 0.125f;

        /// <summary>
        /// If true, the GameObject will be destroyed when the animation finishes.
        /// </summary>
        [Tooltip("If true, the GameObject will be destroyed when the animation finishes."), BoxGroup("Settings")]
        public bool destroyOnFinish;

        /// <summary>
        /// If true, the animation will automatically play on start.
        /// </summary>
        [Tooltip("If true, the animation will automatically play on start."), BoxGroup("Settings")]
        public bool playOnStart;

        /// <summary>
        /// If true, the animation will loop indefinitely.
        /// </summary>
        [Tooltip("If true, the animation will loop indefinitely."), BoxGroup("Settings")]
        public bool loop;

        /// <summary>
        /// If true, the animation will use unscaled time (ignores Time.timeScale).
        /// </summary>
        [Tooltip("If true, the animation will use unscaled time (ignores Time.timeScale)."), BoxGroup("Settings")]
        public bool useUnscaledTime;

        /// <summary>
        /// If true, use Image instead of SpriteRenderer.
        /// </summary>
        [Tooltip("If true, use Image instead of SpriteRenderer."), BoxGroup("Settings")]
        public bool useImage;
        private bool useSpriteRenderer => !useImage;

        /// <summary>
        /// Event triggered when the animation frame changes.
        /// </summary>
        public event Action OnFrameChange;

        /// <summary>
        /// Event triggered when the animation finishes a cycle.
        /// </summary>
        public event Action OnAnimationFinished;

        private Coroutine _play;

        /// <summary>
        /// Gets the target component to animate. Only allows <c>SpriteRenderer</c> or <c>UI.Image</c>.
        /// </summary>
        public Component Target => useImage ? image : spriteRenderer;

        private void Awake()
        {
            if (playOnStart) SetSprite(sprites[0]);
        }

        private void Start()
        {
            if (playOnStart) Play();
        }

        /// <summary>
        /// Starts the animation coroutine.
        /// </summary>
        [ContextMenu("Play")]
        public void Play()
        {
            if (_play != null) StopCoroutine(_play);

            _play = StartCoroutine(PlayCoroutine());
        }

        /// <summary>
        /// Stops the animation coroutine.
        /// </summary>
        public void Stop()
        {
            if (_play == null) return;

            StopCoroutine(_play);
            _play = null;
        }

        private IEnumerator PlayCoroutine()
        {
            do
            {
                foreach (var sprite in sprites)
                {
                    SetSprite(sprite);

                    OnFrameChange?.Invoke();

                    yield return useUnscaledTime ? new WaitForSecondsRealtime(frameDelay) : new WaitForSeconds(frameDelay);

                    OnAnimationFinished?.Invoke();

                    if (destroyOnFinish)
                    {
                        Destroy(gameObject);
                        yield break;
                    }
                }
            } while (loop);
        }

        private void SetSprite(Sprite sprite)
        {
            if (Target is SpriteRenderer sr) sr.sprite = sprite;
            if (Target is Image img) img.sprite = sprite;
        }
    }
}