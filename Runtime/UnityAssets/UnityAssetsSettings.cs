using UnityEngine;
using TypeReferences;
using UnityEngine.InputSystem;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// Stores project settings to alter how this package works.
    /// </summary>
    [System.Serializable]
    public class UnityAssetsSettings : ScriptableObject
    {
        private static UnityAssetsSettings _cached;

        /// <summary>
        /// Type of the developer console, in order for the component to be added.
        /// </summary>
        [Inherits(typeof(AbstractDeveloperConsole), IncludeAdditionalAssemblies = new[] { "Assembly-CSharp" })]
        public TypeReference developerConsoleType;

        /// <summary>
        /// Type of prefab to instantiate for custom scene transition animations.
        /// </summary>
        public GameObject sceneTransitionPrefab;

        /// <summary>
        /// KeyCode to press in order to open the Developer Console.
        /// </summary>
        public Key developerConsoleKey = Key.F3;

        /// <summary>
        /// Text Mesh Pro font asset to use 
        /// </summary>
        public TMP_FontAsset floatingTextFont;

        /// <summary>
        /// List of sprite asset groups to use. First is assigned as the primary.
        /// </summary>
        public TMP_SpriteAsset[] floatingTextEmojiAssets;

        private const string AssetName = "UnityAssetsSettings";
        private const string AssetPath = "Assets/Resources/" + AssetName + ".asset";

        /// <summary>
        /// Get the settings, or create a new set with default values.
        /// </summary>
        /// <returns></returns>
        public static UnityAssetsSettings GetOrCreate()
        {
            if (_cached != null) return _cached;

            // Resources.Load uses a path relative to a Resources folder (no extension)
            _cached = Resources.Load<UnityAssetsSettings>(AssetName);

#if UNITY_EDITOR
            // Only create if not found and not importing assets
            if (_cached == null && !EditorApplication.isUpdating && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _cached = CreateInstance<UnityAssetsSettings>();

                var folder = Path.GetDirectoryName(AssetPath);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                AssetDatabase.CreateAsset(_cached, AssetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
#else
            if (_cached == null)
            {
                Debug.LogWarning("[Lucas's Unity Assets] UnityAssetsSettings not found. Please create it via Project Settings → Unity Assets.");
            }
#endif
            return _cached;
        }
    }
}