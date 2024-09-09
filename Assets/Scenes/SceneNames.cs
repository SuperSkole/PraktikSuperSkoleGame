namespace Scenes
{
    /// <summary>
    /// Static class containing the names of all scenes used in the game.
    /// </summary>
    public static class SceneNames
    {
        // System and initialization scenes
        public const string Boot = "00-Bootstrapper";
        public const string Splash = "01-SplashScene";

        // Login and startup scenes
        public const string Login = "02-LoginScene";
        public const string Start = "03-StartScene";
        public const string Tutorial = "04-TutorialScene";  //

        // Ending and miscellaneous
        public const string Credits = "09-EndingCredits";

        // Player related scenes
        public const string Player = "10-PlayerScene";
        public const string House = "11-PlayerHouse";
        public const string PlayerUIScene = "13-PlayerUIScene";
        public const string Profile = "14-ProfileScene"; //


        // Main gameplay and utilities
        public const string Main = "20-MainWorld";
        public const string Pause = "21-PauseMenu";  //
        public const string Options = "22-OptionScene";  //
        public const string HighScores = "24-HighScoreScene";
        public const string Help = "25-HelpScene";  //
        public const string Story = "26-Cutscene";  //
        public const string Settings = "28-SettingsScene"; //

        // NPC and interaction scenes
        public const string NPCInteractions = "30-NPCInteractionScene"; // might be split out

        public const string Shop = "40-ShopScene";
        public const string Wardrope = "41-WardropeScene";

        // Minigames 50-70
        public const string Gamemode = "50-GameModeSelector";
        public const string LetterLoading = "51-LetterGarden";
        public const string Letter = "52-LetterGarden";
        public const string EaterLoading = "53-SymbolEater";
        public const string Eater = "54-SymbolEater";
        public const string FactoryLoading = "55-WordFactoryLoadingScene";
        public const string Factory = "56-WordFactory";
        public const string RacerLoading = "57-RacingGame";
        public const string Racer = "58-RacingGame";
        public const string BankLoading = "60-BankGame";
        public const string Bank = "61-BankFront";
        public const string BreakinLoading = "62-BreakInGame";
        public const string Breakin = "63-BreakInGame";
        public const string TowerLoading = "64-MonsterTower";
        public const string Tower = "65-MonsterTower";
        public const string MinigameLoading = "LevelSelect";

        //Arcade 70
        public const string Arcade = "70-ArcadeScene";
        public const string ArcadeBalloon = "71-BalloonPopper";
        

        // Multiplayer features
        public const string MultiplayerLobby = "80-MultiplayerLobby";
        public const string Matchmaking = "81-MatchmakingScene";
        public const string MultiPlayerHighScores = "89-HighScoreScene";
        
    }
}
