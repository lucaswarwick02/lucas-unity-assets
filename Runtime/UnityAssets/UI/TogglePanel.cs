using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace LucasWarwick02.UnityAssets
{
    [RequireComponent(typeof(Canvas))]
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

        public RectTransform RectTransform => transform as RectTransform;

        private Vector3 _base;
        private Vector3 _target;

        protected virtual void Awake()
        {
            _base = transform.localScale;
            _target = _base * maxScale;
        }

        [Button]
        public virtual void Open()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(Animation());
            }
            else
            {
                Canvas.enabled = true;
                RectTransform.localScale = _base;
            }
        }


        [Button]
        public virtual void Close()
        {
            if (Application.isPlaying)
            {
                OnClose?.Invoke();
            }

            Canvas.enabled = false;
            RectTransform.localScale = _base;
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
            Canvas.enabled = true;

            OnOpen?.Invoke();

            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float scale = Curves.Bell.Evaluate(t);

                RectTransform.localScale = Vector3.Lerp(_base, _target, scale);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            RectTransform.localScale = _base;
        }
    }
}
