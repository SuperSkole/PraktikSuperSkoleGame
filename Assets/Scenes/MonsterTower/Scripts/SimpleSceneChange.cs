using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChange : MonoBehaviour
{
    public void SwitchToMonsterTårn(string gamemode)
    {
        SceneManager.LoadScene("MonterTower katapult");
        //use gamemode to change which gamemode is used
    }
    public void SwitchToGrovæder(string gamemode)
    {
        SceneManager.LoadScene("Grovæder");
        //use gamemode to change which gamemode is used
    }

}
