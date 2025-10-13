using UnityEngine;
using UnityEngine.Events;

namespace Arcadian.GameObjects
{
    /// <summary>
    /// A reusable Unity component that exposes UnityEvents for common MonoBehaviour lifecycle hooks (<c>Awake</c>, <c>OnEnable</c>, <c>OnDisable</c>). Useful for triggering actions directly in the inspector without writing boilerplate code.
    /// </summary>
    public class UnityEventHooks : MonoBehaviour
    {
        public UnityEvent OnAwake;
        public UnityEvent OnStart;
        public UnityEvent OnEnableEvent;
        public UnityEvent OnDisableEvent;

        private void Awake() => OnAwake?.Invoke();
        private void Start() => OnStart?.Invoke();
        private void OnEnable() => OnEnableEvent?.Invoke();
        private void OnDisable() => OnDisableEvent?.Invoke();
    }
}
