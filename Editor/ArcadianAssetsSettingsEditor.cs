using UnityEditor;

namespace Arcadian.Systems.Editor
{
    public static class ArcadianAssetsSettingsEditor
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Arcadian Assets", SettingsScope.Project)
            {
                label = "Arcadian Assets",
                guiHandler = _ =>
                {
                    var settings = ArcadianAssetsSettings.GetOrCreate();
                    if (settings == null)
                    {
                        EditorGUILayout.HelpBox("ArcadianAssetsSettings asset not found or failed to load.", MessageType.Error);
                        return;
                    }

                    var serializedObject = new SerializedObject(settings);
                    serializedObject.Update();

                    EditorGUILayout.LabelField("Developer Console", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ArcadianAssetsSettings.developerConsoleKey)));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ArcadianAssetsSettings.developerConsoleType)));

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Scene Transitions", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ArcadianAssetsSettings.sceneTransitionPrefab)));

                    EditorGUILayout.LabelField("Floating Text", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ArcadianAssetsSettings.floatingTextFont)));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ArcadianAssetsSettings.floatingTextEmojiAssets)));

                    serializedObject.ApplyModifiedProperties();
                },
                keywords = new[] { "Arcadian", "Assets", "Settings" }
            };

            return provider;
        }
    }
}
