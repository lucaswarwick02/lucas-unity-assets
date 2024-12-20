using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arcadian.UI
{
    [RequireComponent(typeof(Slider))]
    public class ScaleSlider : MonoBehaviour
    {
        private Slider _slider;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            _slider.value = PlayerPrefs.GetFloat(AbstractUI.ScalePlayerPrefsKey, 5f);
            
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            PlayerPrefs.SetFloat(AbstractUI.ScalePlayerPrefsKey, value);
            PlayerPrefs.Save();

            foreach (var abstractUI in FindObjectsByType<AbstractUI>(FindObjectsSortMode.None))
            {
                if (abstractUI.IsClosed) return;
                
                abstractUI.SetScaleAndOffsets();
            }
        }
    }
}