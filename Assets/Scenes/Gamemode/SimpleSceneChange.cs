using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChange : MonoBehaviour
{
     // changes scene as long as there is one of the name
    public void SwitchToScene(string minigame)
    {
        SceneManager.LoadScene(minigame);
    }

}
