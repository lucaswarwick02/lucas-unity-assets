using System.Collections;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// Unity component to allow for single functions to be defined for both gamepad and mouse/keyboard interactions for UI elements such as buttons.
    /// </summary>
    public abstract class InteractiveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        public bool IsFocused { private set; get; }

        /// <summary>
        /// Original scale when not hovered.
        /// </summary>
        [Foldout("Animation")] public Vector3 originalScale = new(1f, 1f, 1f);

        /// <summary>
        /// Maximum scale when hovered.
        /// </summary>
        [Foldout("Animation")] public Vector3 targetScale = new(1.125f, 1.125f, 1.125f);

        /// <summary>
        /// The time taken to increase to the max size.
        /// </summary>
        [Foldout("Animation")] public float duration = 0.0625f;

        /// <summary>
        /// Sound effect to play on enter.
        /// </summary>
        [Header("SFX"), Tooltip("Sound effect to play on enter.")] public SFX enterSFX;

        /// <summary>
        /// Sound effect to play on exit.
        /// </summary>
        [Tooltip("Sound effect to play on exit.")] public SFX exitSFX;

        /// <summary>
        /// Sound effect to play on submit.
        /// </summary>
        [Tooltip("Sound effect to play on submit.")] public SFX submitSFX;

        private Coroutine _animation;

        private void StartAnimation(Vector3 target)
        {
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }

            _animation = StartCoroutine(Animation(target));
        }

        private IEnumerator Animation(Vector3 target)
        {
            var origin = transform.localScale;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(origin, target, elapsed / duration);

                yield return null;
            }

            transform.localScale = target;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsFocused) return;

            IsFocused = true;
            enterSFX.Play();
            StartAnimation(targetScale);
            OnEnter();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (IsFocused) return;

            IsFocused = true;
            enterSFX.Play();
            StartAnimation(targetScale);
            OnEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsFocused) return;

            IsFocused = false;
            exitSFX.Play();
            StartAnimation(originalScale);
            OnExit();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!IsFocused) return;

            IsFocused = false;
            exitSFX.Play();
            StartAnimation(originalScale);
            OnExit();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsFocused) return;

            submitSFX.Play();
            OnSubmit(eventData);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (!IsFocused) return;

            submitSFX.Play();
            OnSubmit();
        }

        /// <summary>
        /// Invoked when the user hovers over the button.
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// Invoked when the user stops hovering over the button.
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// Invoked when the button is pressed/clicked/submitted.
        /// </summary>
        /// <param name="eventData">Optional event data to be passed through.</param>
        public abstract void OnSubmit([CanBeNull] PointerEventData eventData = null);
    }
}