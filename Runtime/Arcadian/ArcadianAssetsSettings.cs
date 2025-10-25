using UnityEngine;
using TypeReferences;
using Arcadian.UI;
using UnityEngine.InputSystem;



#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Arcadian
{
    /// <summary>
    /// Stores project settings to alter how this package works.
    /// </summary>
    [System.Serializable]
    public class ArcadianAssetsSettings : ScriptableObject
    {
        private static ArcadianAssetsSettings _cached;

        /// <summary>
        /// Path to the floating text prefab.
        /// </summary>
        public string floatingTextPath;

        /// <summary>
        /// Path to the transition effect prefab.
        /// </summary>
        public string transitionEffectPath;

        /// <summary>
        /// Type of the developer console, in order for the component to be added.
        /// </summary>
        [Inherits(typeof(AbstractDeveloperConsole), IncludeAdditionalAssemblies = new[] { "Assembly-CSharp" })]
        public TypeReference developerConsoleType;

        /// <summary>
        /// KeyCode to press in order to open the Developer Console.
        /// </summary>
        public Key developerConsoleKey = Key.F3;

        private const string AssetName = "ArcadianAssetsSettings";
        private const string AssetPath = "Assets/Resources/" + AssetName + ".asset";

        /// <summary>
        /// Get the settings, or create a new set with default values.
        /// </summary>
        /// <returns></returns>
        public static ArcadianAssetsSettings GetOrCreate()
        {
            if (_cached != null) return _cached;

            // Resources.Load uses a path relative to a Resources folder (no extension)
            _cached = Resources.Load<ArcadianAssetsSettings>(AssetName);

#if UNITY_EDITOR
            // Only create if not found and not importing assets
            if (_cached == null && !EditorApplication.isUpdating && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                _cached = CreateInstance<ArcadianAssetsSettings>();

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
                Debug.LogWarning("[Arcadian] ArcadianAssetsSettings not found. Please create it via Project Settings → Arcadian Assets.");
            }
#endif
            return _cached;
        }
    }
}