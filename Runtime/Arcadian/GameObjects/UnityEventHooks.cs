using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Arcadian.GameObjects
{
    /// <summary>
    /// A reusable Unity component that exposes UnityEvents for common MonoBehaviour lifecycle hooks (<c>Awake</c>, <c>OnEnable</c>, <c>OnDisable</c>). Useful for triggering actions directly in the inspector without writing boilerplate code.
    /// </summary>
    [AddComponentMenu("Arcadian/GameObjects/Unity Event Hooks")]
    public class UnityEventHooks : MonoBehaviour
    {
        /// <summary>
        /// Event triggered on Unity Awake().
        /// </summary>
        [Tooltip("Event triggered on Unity Awake()."), BoxGroup("Events")]
        public UnityEvent OnAwake;

        /// <summary>
        /// Event triggered on Unity Start().
        /// </summary>
        [Tooltip("Event triggered on Unity Start()."), BoxGroup("Events")]
        public UnityEvent OnStart;

        /// <summary>
        /// Event triggered on Unity OnEnable().
        /// </summary>
        [Tooltip("Event triggered on Unity OnEnable()."), BoxGroup("Events")]
        public UnityEvent OnEnableEvent;

        /// <summary>
        /// Event triggered on Unity OnDisable().
        /// </summary>
        [Tooltip("Event triggered on Unity OnDisable()."), BoxGroup("Events")]
        public UnityEvent OnDisableEvent;

        private void Awake() => OnAwake?.Invoke();
        private void Start() => OnStart?.Invoke();
        private void OnEnable() => OnEnableEvent?.Invoke();
        private void OnDisable() => OnDisableEvent?.Invoke();
    }
}
