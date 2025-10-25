using System.Collections;
using Arcadian.Sound;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcadian.UI
{
    public abstract class AbstractUI : MonoBehaviour
    {
        public const string ScalePlayerPrefsKey = "UIScale";
        
        private const float MinScale = 0.5f;
        private const float MaxScale = 1.5f;
        
        [Header("Abstract UI")]
        [SerializeField] private AbstractUIGroup abstractUIGroup;
        [SerializeField] private SFX openSound;
        [SerializeField] private SFX closeSound;
        [SerializeField] private bool ignoreScaler;

        private const float SizeMultiplier = 1.075f;
        private const float AnimationLength = 0.125f;

        private Coroutine _animation;

        public bool IsOpen => gameObject.activeSelf;

        public bool IsClosed => !gameObject.activeSelf;

        private float GetScale()
        {
            if (transform.parent.GetComponentInParent<AbstractUI>())
            {
                return 1f;
            }
            
            var prefsScale = PlayerPrefs.GetFloat(ScalePlayerPrefsKey, 5f) / 10;
            
            return Mathf.Lerp(MinScale, MaxScale, prefsScale);
        }

        private Vector3 GetLocalScale()
        {
            return Vector3.one * GetScale();
        }

        public void SetScaleAndOffsets()
        {
            if (ignoreScaler) return;
            
            transform.localScale = GetLocalScale();
            
            // Calculate the adjustment needed to fit the scaled panel
            var adjustment = (1f - (1f / GetScale())) / 2f;

            var rectTransform = GetComponent<RectTransform>();
            
            // Apply the adjustment to all sides
            rectTransform.anchorMin = new Vector2(adjustment, adjustment);
            rectTransform.anchorMax = new Vector2(1 - adjustment, 1 - adjustment);
            
            // Reset offsets
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    
        public virtual void Open()
        {
            // Update the UI Scale
            SetScaleAndOffsets();
            
            if (IsOpen) return;

            abstractUIGroup?.CloseOthers(this);

            gameObject.SetActive(true);
            if (openSound) openSound.Play();
            _animation = StartCoroutine(OpenAnimation());
        }

        private IEnumerator OpenAnimation()
        {
            var timer = 0f;

            while (timer < AnimationLength)
            {
                timer += Time.unscaledDeltaTime;
                
                var percentage = Mathf.Sin(timer / AnimationLength * Mathf.PI);
                transform.localScale = GetLocalScale() * Mathf.Lerp(1f, SizeMultiplier, percentage);

                yield return null;
            }

            transform.localScale = GetLocalScale();
        }
    
        public virtual void Close()
        {
            if (IsClosed) return;
            
            if (closeSound) closeSound.Play();
            if (_animation != null) StopCoroutine(_animation);
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }

        protected void OpenAction(InputAction.CallbackContext _) => Open();
        
        protected void CloseAction(InputAction.CallbackContext _) => Close();
    }
}
