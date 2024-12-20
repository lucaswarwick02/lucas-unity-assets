using System.Collections;
using Arcadian.Maths;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arcadian.UI.Effects
{
    public class BubbleSelect : FunctionalButton
    {
        [SerializeField] private float scale = 1.15f;
        [SerializeField] private float speed = 5f;
        
        private Vector3 _originalScale;
        private float _value;
        private Coroutine _coroutine;

        private Vector3 EffectScale => _originalScale * scale;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private IEnumerator Increase()
        {
            while (_value < 1)
            {
                // Update scale
                transform.localScale = Vector3.Lerp(_originalScale, EffectScale, Curves.In.Evaluate(_value));
                
                // Update the stored value
                _value += Time.unscaledDeltaTime * speed;

                yield return null;
            }
            
            // Value has reached the target
            transform.localScale = EffectScale;
            _value = 1;

            _coroutine = null;
        }

        private IEnumerator Decrease()
        {
            while (_value > 0)
            {
                // Update Scale
                transform.localScale = Vector3.Lerp(_originalScale, EffectScale, Curves.In.Evaluate(_value));
                
                // Update the stored value
                _value -= Time.unscaledDeltaTime * speed;

                yield return null;
            }
            
            // Value has reached the target
            transform.localScale = _originalScale;
            _value = 0;

            _coroutine = null;
        }

        public override void OnButtonHover()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Increase());
        }

        public override void OnButtonHoverEnd()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Decrease());
        }

        public override void OnButtonPress(PointerEventData eventData = null)
        {
            //
        }
    }
}