using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Arcadian.Animation
{
    /// <summary>
    /// A reusable Unity component for animating a sequence of sprites on a <c>SpriteRenderer</c> or <c>UI.Image</c>.
    /// Supports looping or one-off animations without using an Animator Controller. 
    /// Allows control over timing, looping, and destruction on completion.
    /// </summary>
    public class AnimationLoop : MonoBehaviour
    {
        /// <summary>
        /// The target component to animate. Must be a <c>SpriteRenderer</c> or <c>UI.Image</c>.
        /// </summary>
        public Component target;

        /// <summary>
        /// Time delay in seconds between frames.
        /// </summary>
        public float frameDelay = 0.125f;

        /// <summary>
        /// The sequence of sprites to animate.
        /// </summary>
        public Sprite[] sprites;

        /// <summary>
        /// If true, the GameObject will be destroyed when the animation finishes.
        /// </summary>
        public bool destroyOnFinish;

        /// <summary>
        /// If true, the animation will automatically play on start.
        /// </summary>
        public bool playOnStart;

        /// <summary>
        /// If true, the animation will loop indefinitely.
        /// </summary>
        public bool loop;

        /// <summary>
        /// If true, the animation will use unscaled time (ignores Time.timeScale).
        /// </summary>
        public bool useUnscaledTime;

        /// <summary>
        /// Event triggered when the animation frame changes.
        /// </summary>
        public event Action OnFrameChange;

        /// <summary>
        /// Event triggered when the animation finishes a cycle.
        /// </summary>
        public event Action OnAnimationFinished;

        private Coroutine _play;

        /// <summary>
        /// Gets or sets the target component to animate. Only allows <c>SpriteRenderer</c> or <c>UI.Image</c>.
        /// </summary>
        public Component Target
        {
            get => target;
            set
            {
                if (value is SpriteRenderer || value is Image || value == null)
                {
                    target = value;
                }
            }
        }

        private void Awake()
        {
            if (playOnStart) SetSprite(sprites[0]);
        }

        private void Start()
        {
            if (playOnStart) Play();
        }

        /// <summary>
        /// Starts the animation coroutine.
        /// </summary>
        [ContextMenu("Play")]
        public void Play()
        {
            if (_play != null) StopCoroutine(_play);

            _play = StartCoroutine(PlayCoroutine());
        }

        /// <summary>
        /// Stops the animation coroutine.
        /// </summary>
        public void Stop()
        {
            if (_play == null) return;

            StopCoroutine(_play);
            _play = null;
        }

        private IEnumerator PlayCoroutine()
        {
            do
            {
                foreach (var sprite in sprites)
                {
                    SetSprite(sprite);

                    OnFrameChange?.Invoke();

                    yield return useUnscaledTime ? new WaitForSecondsRealtime(frameDelay) : new WaitForSeconds(frameDelay);

                    OnAnimationFinished?.Invoke();

                    if (destroyOnFinish)
                    {
                        Destroy(gameObject);
                        yield break;
                    }
                }
            } while (loop);
        }

        private void SetSprite(Sprite sprite)
        {
            if (target is SpriteRenderer sr) sr.sprite = sprite;
            if (target is Image img) img.sprite = sprite;
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// Custom editor for <c>AnimationLoop</c> to handle inspector validation and display.
    /// </summary>
    [CustomEditor(typeof(AnimationLoop))]
    public class AnimationLoopEditor : Editor
    {
        /// <summary>
        /// Draws the custom inspector GUI for <c>AnimationLoop</c> .
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var script = serializedObject.FindProperty("m_Script");
            EditorGUI.BeginDisabledGroup(true); // Script field is read-only
            EditorGUILayout.PropertyField(script);
            EditorGUI.EndDisabledGroup();

            var targetProp = serializedObject.FindProperty("target");
            EditorGUI.BeginChangeCheck();
            var obj = EditorGUILayout.ObjectField("Target", targetProp.objectReferenceValue, typeof(Component), true);

            if (obj == null || obj is SpriteRenderer || obj is Image)
                targetProp.objectReferenceValue = obj;
            else
                EditorGUILayout.HelpBox("Target must be a SpriteRenderer or Image.", MessageType.Warning);

            // Draw remaining fields manually
            EditorGUILayout.PropertyField(serializedObject.FindProperty("frameDelay"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sprites"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyOnFinish"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playOnStart"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useUnscaledTime"));

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

}