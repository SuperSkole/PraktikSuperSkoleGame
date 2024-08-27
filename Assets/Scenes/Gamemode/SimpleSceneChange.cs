using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChange : MonoBehaviour
{
     /// <summary>
     /// changes scene as long as there is one of the name
     /// </summary>
     /// <param name="minigame">the name of the scene to switch to</param>
    public void SwitchToScene(string minigame)
    {
        SceneManager.LoadScene(minigame);
    }

}
