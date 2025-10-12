using Arcadian.Maths;
using UnityEngine;

namespace Arcadian.Effects
{
    /// <summary>
    /// A simple Unity component that makes a GameObject smoothly pulse in size. Useful for drawing attention to UI elements or objects with a looping, easing-based scale effect.
    /// </summary>
    public class Pulse : MonoBehaviour
    {
        [SerializeField] private float speed = 5;
        [SerializeField] private float scale = 1.15f;

        private void Update()
        {
            var t = Curves.In.Evaluate(Mathf.PingPong(Time.time * speed, 1f));
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, t);
        }

        private void OnDisable()
        {
            transform.localScale = Vector3.one;
        }
    }
}