using CORE.Scripts;
using UnityEngine;

namespace SceneManagement
{
    public class LoginSceneState : ISceneState
    {
        public void OnEnter()
        {
            Debug.Log("Entering Login Scene");
            
            // Assuming DataLoader is attached to a GameObject named 'DataLoaderObject'
            GameObject dataLoaderObject = GameObject.Find("DataLoaderObject");
            if (dataLoaderObject != null)
            {
                DataLoader dataLoader = dataLoaderObject.GetComponent<DataLoader>();
                if (dataLoader != null)
                {
                    dataLoader.Start(); // Start the loading process
                }
                else
                {
                    UnityEngine.Debug.LogError("DataLoader component not found on DataLoaderObject.");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("DataLoaderObject not found in the scene.");
            }
        }

        public void OnExit()
        {
            Debug.Log("Exiting Login Scene");
            // Possibly cleanup UI and services
        }

        public void UpdateState()
        {
            // Handle login input and authentication logic
        }
    }
}