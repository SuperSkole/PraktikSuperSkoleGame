using System.Collections.Generic;
using Scenes._10_PlayerScene.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    ///// <summary>
    ///// Adds a user specified script to the player on awake and the removes it later on sceneunload
    ///// </summary>
    //public class ComponetAddMiniGameSceneStart : MonoBehaviour
    //{
    //    // Select the script to be added to the player in the Inspector
    //    //[SerializeField] private MonoBehaviour scriptToAdd;
        
    //    [SerializeField] private List<MonoScript> scriptToAdd;

    //    // Reference to the added component for later removal
    //    private List<MonoBehaviour> addedComponents;

    //    private void Awake()
    //    {
    //        addedComponents = new List<MonoBehaviour>();

    //        if (PlayerManager.Instance.SpawnedPlayer != null)
    //        {
    //            //addedComponent = (MonoBehaviour)PlayerManager.Instance.SpawnedPlayer.AddComponent(scriptToAdd.GetType());
    //            foreach (var script in scriptToAdd)
    //            {
    //                MonoBehaviour addedComponent = (MonoBehaviour)PlayerManager.Instance.SpawnedPlayer.AddComponent(script.GetClass());
    //                addedComponents.Add(addedComponent);
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Player or ScriptToAdd is not set.");
    //        }
    //        // Subscribe to the scene unloading event
    //        SceneManager.sceneUnloaded += OnSceneUnloaded;
    //    }
    
    //    /// <summary>
    //    /// This method to removes the added component from Awake
    //    /// </summary>
    //    private void RemoveAddedComponent()
    //    {
    //        if (addedComponents != null)
    //        {
    //            foreach (var component in addedComponents)
    //            {
    //                if (component != null)
    //                {
    //                    Destroy(component);
    //                }
    //            }
    //            addedComponents.Clear(); // Clear the list after removing all components
    //        }
    //        else
    //        {
    //            Debug.LogWarning("No components to remove.");
    //        }
    //    }
    //    /// <summary>
    //    /// Called when the scene is unloaded
    //    /// </summary>
    //    /// <param name="current"></param>
    //    private void OnSceneUnloaded(Scene current)
    //    {
    //        // Remove the added component before the scene unloads
    //        RemoveAddedComponent();
    //        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    //    }

    //}
}