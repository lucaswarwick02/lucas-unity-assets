#if UNITY_EDITOR
using UnityEditor;

namespace LucasWarwick02.UnityAssets.Editor
{
    public static class UnityAssetsSettingsEditor
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Lucas's Unity Assets", SettingsScope.Project)
            {
                label = "Lucas's Unity Assets",
                guiHandler = _ =>
                {
                    var settings = UnityAssetsSettings.GetOrCreate();
                    if (settings == null)
                    {
                        EditorGUILayout.HelpBox("[Lucas's Unity Assets] UnityAssetsSettings asset not found or failed to load.", MessageType.Error);
                        return;
                    }

                    var serializedObject = new SerializedObject(settings);
                    serializedObject.Update();

                    EditorGUILayout.LabelField("Developer Console", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnityAssetsSettings.developerConsoleKey)));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnityAssetsSettings.developerConsoleType)));

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Scene Transitions", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnityAssetsSettings.sceneTransitionPrefab)));
                    
                    EditorGUILayout.LabelField("Floating Text", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnityAssetsSettings.floatingTextFont)));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnityAssetsSettings.floatingTextEmojiAssets)));

                    EditorGUILayout.LabelField("Input System", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(UnityAssetsSettings.inputActionsType)));


                    serializedObject.ApplyModifiedProperties();
                },
                keywords = new[] { "Lucas", "Unity", "Assets", "Settings" }
            };

            return provider;
        }
    }
}
#endif