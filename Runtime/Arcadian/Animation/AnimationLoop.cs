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
    /// Contains a single animation and loops it.
    /// </summary>
    public class AnimationLoop : MonoBehaviour
    {
        public Component target;
        
        public float frameDelay = 0.125f;
        public Sprite[] sprites;

        public bool destroyOnFinish;
        public bool playOnStart;
        public bool loop;
        public bool useUnscaledTime;

        public event Action onAnimationFinished;

        private Coroutine _play;

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
        /// Starts the PlayCoroutine.
        /// </summary>
        [ContextMenu("Play")]
        public void Play()
        {
            if (_play != null) StopCoroutine(_play);

            _play = StartCoroutine(PlayCoroutine());
        }

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
                    yield return useUnscaledTime ? new WaitForSecondsRealtime(frameDelay) : new WaitForSeconds(frameDelay);

                    onAnimationFinished?.Invoke();

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
    [CustomEditor(typeof(AnimationLoop))]
    public class AnimationLoopEditor : Editor
    {
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