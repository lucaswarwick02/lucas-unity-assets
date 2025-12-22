using UnityEngine;
using UnityEngine.InputSystem;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// Tracks the user's currently active input device (keyboard/mouse or gamepad)
    /// based on the most recent input action that was triggered.
    /// Persists across scenes and auto-instantiates at application startup.
    /// </summary>
    public sealed class InputDeviceTracker : MonoBehaviour
    {
        /// <summary>
        /// High-level classification of supported input device types.
        /// </summary>
        public enum InputDeviceType
        {
            KeyboardMouse,
            Gamepad,
        }

        /// <summary>
        /// The most recently used input device type.
        /// </summary>
        public static InputDeviceType CurrentDevice { get; private set; }

        /// <summary>
        /// Raised whenever the active input device type changes.
        /// </summary>
        public static event System.Action<InputDeviceType> DeviceChanged;

        private static InputDeviceTracker _instance;

        private static bool HasInstance => _instance != null && _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (HasInstance)
                return;

            var go = new GameObject("[Lucas's Unity Assets] Input Device Tracker");
            _instance = go.AddComponent<InputDeviceTracker>();
            DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            if (HasInstance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            SetInitialDevice();
            InputSystem.onActionChange += OnActionChange;
        }

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;

            InputSystem.onActionChange -= OnActionChange;
        }

        private static void SetInitialDevice()
        {
            // Priority: Gamepad > Keyboard & Mouse
            if (Gamepad.current != null)
                SetDevice(InputDeviceType.Gamepad);
            else if (Keyboard.current != null || Mouse.current != null)
                SetDevice(InputDeviceType.KeyboardMouse);

        }

        private static void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.ActionStarted)
                return;

            if (obj is not InputAction action)
                return;

            var control = action.activeControl;
            if (control == null)
                return;

            var device = control.device;

            if (device is Gamepad)
                SetDevice(InputDeviceType.Gamepad);
            else if (device is Keyboard || device is Mouse)
                SetDevice(InputDeviceType.KeyboardMouse);
        }

        private static void SetDevice(InputDeviceType device)
        {
            if (CurrentDevice == device)
                return;

            CurrentDevice = device;
            DeviceChanged?.Invoke(device);
        }
    }
}
