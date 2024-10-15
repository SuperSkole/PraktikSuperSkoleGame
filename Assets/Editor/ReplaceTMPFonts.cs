using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ReplaceTMPFonts : EditorWindow
{
    TMP_FontAsset newFontAsset;
    private string comicSansPath = "Assets/Fonts/ComicSansTMPFont"; // Ensure this path is correct

    [MenuItem("Tools/Replace Fonts in TextMeshPro")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceTMPFonts>("Replace TMP Fonts");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace Fonts in TextMeshPro Components", EditorStyles.boldLabel);
        newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font Asset", newFontAsset, typeof(TMP_FontAsset), false);

        // Load default Comic Sans if no font asset is provided
        if (GUILayout.Button("Set Default to Comic Sans"))
        {
            newFontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(comicSansPath);
            if (newFontAsset == null)
            {
                EditorUtility.DisplayDialog("Error", "Comic Sans Font Asset not found. Please ensure it's in the correct path.", "OK");
                return;
            }
        }

        if (GUILayout.Button("Replace TMP Fonts"))
        {
            if (newFontAsset == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a new TMP Font Asset or set the default to Comic Sans.", "OK");
                return;
            }

            ReplaceFontsInAllScenes();
        }
    }

    void ReplaceFontsInAllScenes()
    {
        // Save the currently open scene to avoid losing progress
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        // Get all scenes in the project
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            Scene scene = EditorSceneManager.OpenScene(scenePath);

            ReplaceAllTMPFonts(scene);

            // Save the scene after making changes
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        EditorUtility.DisplayDialog("Fonts Replaced", "Replaced fonts in all scenes.", "OK");
    }

    void ReplaceAllTMPFonts(Scene scene)
    {
        TMP_Text[] texts = Resources.FindObjectsOfTypeAll<TMP_Text>();
        int count = 0;

        foreach (TMP_Text text in texts)
        {
            if (text.font != newFontAsset)
            {
                Undo.RecordObject(text, "Replace TMP Font");
                text.font = newFontAsset;
                EditorUtility.SetDirty(text);
                count++;
            }
        }

        Debug.Log($"Replaced fonts in {count} TextMeshPro components in scene {scene.name}.");
    }
}
