using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComponetAddMiniGameSceneStart : MonoBehaviour
{
    // Select the script to be added to the player in the Inspector
    //[SerializeField] private MonoBehaviour scriptToAdd;
    [SerializeField] private MonoScript scriptToAdd;

    // Reference to the added component for later removal
    private MonoBehaviour addedComponent;

    private void Awake()
    {
        // Ensure both fields are set
        if (PlayerManager.Instance.SpawnedPlayer != null && scriptToAdd != null)
        {
            // Add the selected script as a component to the player GameObject
            //addedComponent = (MonoBehaviour)PlayerManager.Instance.SpawnedPlayer.AddComponent(scriptToAdd.GetType());
            addedComponent = (MonoBehaviour)PlayerManager.Instance.SpawnedPlayer.AddComponent(scriptToAdd.GetClass());
        }
        else
        {
            Debug.LogWarning("Player or ScriptToAdd is not set.");
        }
        // Subscribe to the scene unloading event
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    /// <summary>
    /// Call this method to remove the added component
    /// </summary>
    private void RemoveAddedComponent()
    {
        if (addedComponent != null)
        {
            Destroy(addedComponent);
            addedComponent = null; //Clear the reference to the removed component
        }
        else
        {
            Debug.LogWarning("No component to remove.");
        }
    }
    /// <summary>
    /// This method is called when the scene is unloaded
    /// </summary>
    /// <param name="current"></param>
    private void OnSceneUnloaded(Scene current)
    {
        // Remove the added component before the scene unloads
        RemoveAddedComponent();
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

}