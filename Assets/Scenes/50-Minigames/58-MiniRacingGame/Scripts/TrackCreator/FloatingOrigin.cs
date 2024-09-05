using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    /// <summary>
    /// Returns the player and world to point 0 in the world, to avoid glitches if they drive too far
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class FloatingOrigin : MonoBehaviour
    {
        public float threshold = 100.0f;
        public LevelLayoutGenerator layoutgenerator;

        private void LateUpdate()
        {
            Vector3 cameraPosition = gameObject.transform.position;
            cameraPosition.y = 0f;

            if (cameraPosition.magnitude > threshold)
            {
                for (int z = 0; z < SceneManager.sceneCount; z++)
                {
                    foreach (GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects())
                    {
                        g.transform.position -= cameraPosition;
                    }
                }

                Vector3 originDelta = Vector3.zero - cameraPosition;
                layoutgenerator.UpdateSpawnOrigin(originDelta);
                Debug.Log("recentering, origin delta = " + originDelta);
            }
        }
    }
}
