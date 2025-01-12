using System;
using UnityEngine;
using UnityEngine.Events;

namespace Arcadian.GameObjects
{
    public class UnityEvents : MonoBehaviour
    {
        public UnityEvent OnAwake;

        public UnityEvent OnStart;

        private void Awake()
        {
            OnAwake?.Invoke();
        }

        private void Start()
        {
            OnStart?.Invoke();
        }
    }
}