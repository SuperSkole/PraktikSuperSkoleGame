using Unity.Netcode;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static void LoadNetwork(string scene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
