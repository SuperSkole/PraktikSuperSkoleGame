using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChange : MonoBehaviour
{
    //changes scene to monstertower
    public void SwitchToMonsterT�rn(string gamemode)
    {
        SceneManager.LoadScene("MonterTower katapult");
    }
    // changes scene to grov�der
    public void SwitchToGrov�der(string gamemode)
    {
        SceneManager.LoadScene("Grov�der");
    }

}
