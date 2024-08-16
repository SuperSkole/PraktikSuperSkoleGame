using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChange : MonoBehaviour
{
    //changes scene to monstertower
    public void SwitchToMonsterTårn(string gamemode)
    {
        SceneManager.LoadScene("MonterTower katapult");
    }
    // changes scene to grovæder
    public void SwitchToGrovæder(string gamemode)
    {
        SceneManager.LoadScene("Grovæder");
    }

}
