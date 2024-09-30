using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SwitchScenes : MonoBehaviour
    {   
        public static void SwitchToMainWorld() => SceneManager.LoadScene(SceneNames.Main);
        public static void SwitchToPlayerHouseScene() => SceneManager.LoadScene(SceneNames.House);
        public static void SwitchToWordFactoryLoadingScene() => SceneManager.LoadScene(SceneNames.FactoryLoading);
        public static void SwitchToArcadeAsteroidScene() => SceneManager.LoadScene(SceneNames.ArcadeAsteroid);
        public static void SwitchToArcadeAsteroidLoseScene() => SceneManager.LoadScene(SceneNames.ArcadeAsteroidLoseScreen);
        public static void SwitchToPathOfDanger() => SceneManager.LoadScene(SceneNames.PathOfDanger);
        public static void SwitchToPathOfDangerLoseScene() => SceneManager.LoadScene(SceneNames.PathOfDangerLoseScreen);
        public static void SwitchToWordFactory() => SceneManager.LoadScene(SceneNames.Factory);
        public static void SwitchToRacingScene() => SceneManager.LoadScene(SceneNames.House);    
        public static void SwitchToSymbolEaterScene() => SceneManager.LoadScene(SceneNames.Eater);    
        public static void SwitchToSymbolEaterLoaderScene() => SceneManager.LoadScene(SceneNames.EaterLoading);
        public static void SwitchToTowerScene() => SceneManager.LoadScene(SceneNames.Tower);
        public static void SwitchToTowerLoaderScene() => SceneManager.LoadScene(SceneNames.TowerLoading);
        public static void SwitchToRacerLoaderScene() => SceneManager.LoadScene(SceneNames.RacerLoading);
        public static void SwitchToRacerScene() => SceneManager.LoadScene(SceneNames.Racer);
        public static void SwitchToArcadeScene() => SceneManager.LoadScene(SceneNames.Arcade);
        public static void SwitchToArcadeBalloonScene() => SceneManager.LoadScene(SceneNames.ArcadeBalloon);
        public static void SwitchToArcadeCatClock() => SceneManager.LoadScene(SceneNames.ArcadeCatClock);
        public static void SwitchToLetterGardenLoaderScene() => SceneManager.LoadScene(SceneNames.LetterLoading);
        public static void SwitchToLetterGardenScene() => SceneManager.LoadScene(SceneNames.Letter);
        public static void SwitchToBankFrontScene() => SceneManager.LoadScene(SceneNames.Bank);
        public static void SwitchToBankBackScene() => SceneManager.LoadScene(SceneNames.BankBack);
        public static void SwitchToMinigameLoadingScene() => SceneManager.LoadScene(SceneNames.MinigameLoading);
        public static void SwitchToMechnicView() => SceneManager.LoadScene(SceneNames.CarShowCaseRoom);
        public static void SwitchToHighscore() => SceneManager.LoadScene(SceneNames.HighScores);
        public static void SwitchToLeaderBoard() => SceneManager.LoadScene(SceneNames.LeaderBoard);
        public static void SwitchToBankFrontLoadingScene() => SceneManager.LoadScene(SceneNames.BankFrontLoading);
        public static void SwitchToAllTimeHighscore() => SceneManager.LoadScene(SceneNames.MultiPlayerHighScores);
    }
}
