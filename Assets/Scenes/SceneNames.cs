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
        public const string Player = "20-PlayerScene";  
        public const string House = "21-PlayerHouse";
        public const string Profile = "23-ProfileScene"; //
        
        
        // Main gameplay and utilities
        public const string Main = "30-MainWorld";
        public const string Pause = "31-PauseMenu";  //
        public const string Options = "32-OptionScene";  //
        public const string HighScores = "34-HighScoreScene"; 
        public const string Help = "35-HelpScene";  //
        public const string Story = "36-Cutscene";  //
        public const string Settings = "38-SettingsScene"; //
        
        // NPC and interaction scenes
        public const string NPCInteractions = "40-NPCInteractionScene"; // might be split out
        public const string Shop = "41-ShopScene";
        public const string Wardrope = "42-WardropeScene";  
        
        // Minigames 50-70
        public const string Gamemode = "50-GameModeSelector";
        public const string Letter = "51-LetterGardenScene";
        public const string Eater = "52-SymbolEater";
        public const string Factory = "53-WordFactory";
        public const string Racer = "55-RacingGame";
        public const string Bank = "56-BankGame";
        public const string Breakin = "57-BreakInGame";
        public const string Tower = "59-MonsterTower";

        // Multiplayer features
        public const string MultiplayerLobby = "80-MultiplayerLobby";
        public const string Matchmaking = "81-MatchmakingScene";
        public const string MultiPlayerHighScores = "89-HighScoreScene"; 

        
    }
}
