using UnityEditor;
using UnityEngine;
using Arcadian;

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

                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ArcadianAssetsSettings.floatingTextPath)));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ArcadianAssetsSettings.transitionEffectPath)));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ArcadianAssetsSettings.soundEffectPath)));

                    serializedObject.ApplyModifiedProperties();
                },
                keywords = new[] { "Arcadian", "Assets", "Settings" }
            };

            return provider;
        }
    }
}
