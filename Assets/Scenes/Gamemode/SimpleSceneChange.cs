using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChange : MonoBehaviour
{
    //changes scene to monstertower
    public void SwitchToMonsterTower(string gamemode)
    {
        SceneManager.LoadScene("MonterTower katapult");
    }
    // changes scene to grovæder
    public void SwitchToSymbolEater(string gamemode)
    {
        SceneManager.LoadScene("SymbolEater");
    }

}
