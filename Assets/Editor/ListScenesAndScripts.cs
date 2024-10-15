using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class ListSceneScriptsByFolder : EditorWindow
{
    [MenuItem("Tools/List Scene Scripts By Folder")]
    public static void ShowWindow()
    {
        GetWindow<ListSceneScriptsByFolder>("List Scene Scripts By Folder");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate Scene Scripts List"))
        {
            GenerateSceneScriptsList();
        }
    }

    private static void GenerateSceneScriptsList()
    {
        // Define the scenes folder and scripts folder
        string scenesFolder = "Assets/Scenes";
        string scriptsFolder = "Assets/Scripts";

        // Regular expression to match scene folder names starting with 00-99
        Regex sceneNameRegex = new Regex(@"^[0-9]{2}-");

        // Prepare data structure to hold scripts organized by scene and folders
        Dictionary<string, Dictionary<string, List<string>>> sceneScripts = new Dictionary<string, Dictionary<string, List<string>>>();

        int totalScenes = 0;
        HashSet<string> uniqueScripts = new HashSet<string>();

        // --- Process Scene Folders ---

        // Get all directories under the Scenes folder whose names start with 00-99
        var sceneDirectories = Directory.GetDirectories(scenesFolder, "*", SearchOption.AllDirectories)
            .Where(dir => sceneNameRegex.IsMatch(Path.GetFileName(dir)))
            .ToList();

        totalScenes = sceneDirectories.Count;

        foreach (string sceneDir in sceneDirectories)
        {
            string sceneName = Path.GetFileName(sceneDir);

            // Find all .cs files in this scene folder and its subfolders
            var csFiles = Directory.GetFiles(sceneDir, "*.cs", SearchOption.AllDirectories)
                .Where(path => !IsInExcludedFolder(path))
                .Select(path => path.Replace("\\", "/"))
                .ToList();

            // Organize scripts by folder
            Dictionary<string, List<string>> scriptsByFolder = OrganizeScriptsByFolder(csFiles, sceneDir, sceneName);

            // Add scripts to the unique scripts set
            foreach (var script in csFiles)
            {
                uniqueScripts.Add(Path.GetFullPath(script));
            }

            sceneScripts[sceneName] = scriptsByFolder;
        }

        // --- Process Scripts Folder ---

        // Check if the scripts folder exists
        if (Directory.Exists(scriptsFolder))
        {
            // Find all .cs files in the Scripts folder and its subfolders
            var csFiles = Directory.GetFiles(scriptsFolder, "*.cs", SearchOption.AllDirectories)
                .Where(path => !IsInExcludedFolder(path))
                .Select(path => path.Replace("\\", "/"))
                .ToList();

            // Organize scripts by folder
            Dictionary<string, List<string>> scriptsByFolder = OrganizeScriptsByFolder(csFiles, scriptsFolder, "Scripts");

            // Add scripts to the unique scripts set
            foreach (var script in csFiles)
            {
                uniqueScripts.Add(Path.GetFullPath(script));
            }

            // Add to the main dictionary with the key "Scripts"
            sceneScripts["Scripts"] = scriptsByFolder;
        }

        int totalScripts = uniqueScripts.Count;

        // *** Output the results to a text file ***
        string outputPath = "Assets/Editor/SceneScriptsList.txt";
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            writer.WriteLine($"Total Scenes: {totalScenes}");
            writer.WriteLine($"Total Scripts: {totalScripts}");
            writer.WriteLine();

            foreach (var sceneEntry in sceneScripts)
            {
                string sceneName = sceneEntry.Key;
                var scriptsByFolder = sceneEntry.Value;

                int sceneScriptCount = scriptsByFolder.Values.Sum(list => list.Count);

                writer.WriteLine($"{sceneName} ({sceneScriptCount} scripts)");

                var sortedFolders = scriptsByFolder.Keys.ToList();
                sortedFolders.Sort();

                foreach (var folder in sortedFolders)
                {
                    int folderScriptCount = scriptsByFolder[folder].Count;
                    string indent = "\t";

                    writer.WriteLine($"{indent}{folder} ({folderScriptCount} scripts)");

                    foreach (string scriptName in scriptsByFolder[folder])
                    {
                        writer.WriteLine($"{indent}\t{scriptName}");
                    }
                }

                writer.WriteLine(); // Blank line between scenes
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", $"Scene scripts list generated at {outputPath}", "OK");
    }

    private static Dictionary<string, List<string>> OrganizeScriptsByFolder(List<string> csFiles, string baseDir, string rootName)
    {
        Dictionary<string, List<string>> scriptsByFolder = new Dictionary<string, List<string>>();

        foreach (string csFile in csFiles)
        {
            // Get folder relative to the base directory
            string relativeFolder = Path.GetDirectoryName(csFile).Replace("\\", "/");
            if (relativeFolder.StartsWith(baseDir))
            {
                relativeFolder = relativeFolder.Substring(baseDir.Length).TrimStart('/');
            }

            // Use root name if script is directly under the base directory
            if (string.IsNullOrEmpty(relativeFolder))
            {
                relativeFolder = rootName;
            }
            else
            {
                // Prepend root name to maintain hierarchy
                relativeFolder = $"{rootName}/{relativeFolder}";
            }

            // Add script to the folder
            if (!scriptsByFolder.ContainsKey(relativeFolder))
            {
                scriptsByFolder[relativeFolder] = new List<string>();
            }
            scriptsByFolder[relativeFolder].Add(Path.GetFileName(csFile));
        }

        // Sort scripts within each folder
        foreach (var folder in scriptsByFolder.Keys.ToList())
        {
            scriptsByFolder[folder].Sort();
        }

        return scriptsByFolder;
    }

    private static bool IsInExcludedFolder(string path)
    {
        // Exclude scripts in certain folders, e.g., Editor, Plugins, Packages
        string[] excludedFolders = { "/Editor/", "/Plugins/", "/Packages/", "/ThirdParty/" };

        foreach (var excluded in excludedFolders)
        {
            if (path.Replace("\\", "/").Contains(excluded))
            {
                return true;
            }
        }
        return false;
    }
}
