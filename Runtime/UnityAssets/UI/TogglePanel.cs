using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace LucasWarwick02.UnityAssets
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup), typeof(GraphicRaycaster))]
    public class TogglePanel : MonoBehaviour
    {
        [Foldout("Toggle Panel Settings")]
        [SerializeField] private float duration = 0.125f;

        [Foldout("Toggle Panel Settings")]
        [SerializeField] private float maxScale = 1.15f;

        public event Action OnOpen;
        public event Action OnClose;

        private Canvas _canvas;
        public Canvas Canvas => _canvas ? _canvas : _canvas = GetComponent<Canvas>();

        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => _canvasGroup ? _canvasGroup : _canvasGroup = GetComponent<CanvasGroup>();

        public RectTransform RectTransform => transform as RectTransform;

        private Vector3 _target = Vector3.one;

        public bool IsOpen { private set; get;}

        protected virtual void Awake()
        {
            _target = Vector3.one * maxScale;
        }

        [ContextMenu("Open")]
        public virtual void Open()
        {
            Canvas.enabled = true;
            RectTransform.localScale = Vector3.one;
            CanvasGroup.interactable = true;

            IsOpen = true;

            if (Application.isPlaying)
            {
                StartCoroutine(Animation());
                OnOpen?.Invoke();
            }
        }


        [ContextMenu("Close")]
        public virtual void Close()
        {
            Canvas.enabled = false;
            RectTransform.localScale = Vector3.one;
            CanvasGroup.interactable = false;

            IsOpen = false;


            if (Application.isPlaying)
            {
                OnClose?.Invoke();
            }
        }

        public void Toggle()
        {
            if (Canvas.enabled)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        private IEnumerator Animation()
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float scale = Curves.Bell.Evaluate(t);

                RectTransform.localScale = Vector3.Lerp(Vector3.one, _target, scale);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            RectTransform.localScale = Vector3.one;
        }
    }
}
