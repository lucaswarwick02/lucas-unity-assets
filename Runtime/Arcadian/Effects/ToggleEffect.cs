using System.Collections;
using Arcadian.Extensions;
using Arcadian.Maths;
using Arcadian.Sound;
using UnityEditor.Rendering;
using UnityEngine;

namespace Arcadian.Effects
{
    public class ToggleEffect : MonoBehaviour
    {
        private const float AnimationLengthSeconds = 0.125f;
        private const float ScaleMultiplier = 1.075f;

        public SFX openSFX;
        public SFX closeSFX;

        private bool _isQuitting;
        private Vector3 _originalScale;
        private Coroutine _animation;

        void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void OnEnable()
        {
            openSFX.Play();

            if (_animation != null) StopCoroutine(_animation);
            _animation = StartCoroutine(Animation);
        }

        private void OnDisable()
        {
            if (_isQuitting) return;

            closeSFX.Play();
            if (_animation != null) StopCoroutine(Animation);
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private IEnumerator Animation => IEnumeratorExtensions.Tween(
            duration: AnimationLengthSeconds,
            onStart: () =>
            {
                transform.localScale = _originalScale;
            },
            onUpdate: t =>
            {
                transform.localScale = Vector3.Lerp(_originalScale, _originalScale * ScaleMultiplier, t);
            },
            onComplete: () =>
            {
                transform.localScale = _originalScale;
            },
            curve: Curves.Bell
        );
    }
}