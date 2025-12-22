using System.Collections.Generic;
using System.Linq;
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
        /// The currently active PlayerInput control scheme.
        /// </summary>
        public static string CurrentScheme { get; private set; }

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

            if (UnityAssetsSettings.GetOrCreate().inputActionsType == null)
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
            if (Gamepad.current != null)
                SetDevice(InputDeviceType.Gamepad);
            else if (Keyboard.current != null || Mouse.current != null)
                SetDevice(InputDeviceType.KeyboardMouse);
        }

        private static void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.ActionStarted)
                return;

            if (obj is not InputAction action || action.activeControl == null)
                return;

            var device = action.activeControl.device;
            if (device is Gamepad)
                SetDevice(InputDeviceType.Gamepad);
            else if (device is Keyboard || device is Mouse)
                SetDevice(InputDeviceType.KeyboardMouse);

            var playerInput = FindObjectOfType<PlayerInput>();
            if (playerInput != null)
                CurrentScheme = playerInput.currentControlScheme;
        }

        private static string[] GetDeviceStrings(InputDeviceType inputDeviceType)
        {
            return inputDeviceType switch
            {
                InputDeviceType.KeyboardMouse => new string[2] { "Mouse", "Keyboard" },
                InputDeviceType.Gamepad => new string[1] { "Gamepad" },
                _ => throw new System.NotImplementedException(),
            };
        }

        private static string InferControlScheme()
        {
            var controlSchemes = UnityAssetsSettings.GetOrCreate().inputActionsType.controlSchemes;
            if (controlSchemes.Count == 0)
                return null;

            var requiredDevices = new HashSet<string>(GetDeviceStrings(CurrentDevice));
            
            foreach(var controlScheme in controlSchemes)
            {
                if (controlScheme.deviceRequirements.All(req => requiredDevices.Any(device => req.controlPath.Contains(device))))
                {
                    return controlScheme.name;
                }
            }

            return null;
        }

        private static void SetDevice(InputDeviceType device)
        {
            if (CurrentDevice == device && CurrentScheme != null)
                return;

            CurrentDevice = device;
            CurrentScheme = InferControlScheme();
            DeviceChanged?.Invoke(device);
        }
    }

    public static class InputActionExtensions
    {
        /// <summary>
        /// Gets the display string for the current control scheme's binding.
        /// </summary>
        /// <param name="inputAction">The input action to get the binding string for.</param>
        /// <returns>A formatted string representing the binding, or an empty string if no matching binding is found.</returns>
        public static string GetBindingString(this InputAction inputAction)
        {
            var display = inputAction.bindings.FirstOrDefault(b => b.groups.Equals(InputDeviceTracker.CurrentScheme)).ToDisplayString();
            var mappings = new Dictionary<string, string>{};

            if (mappings.TryGetValue(display, out var friendly))
                return friendly;

            return display;
        }
    }
}
