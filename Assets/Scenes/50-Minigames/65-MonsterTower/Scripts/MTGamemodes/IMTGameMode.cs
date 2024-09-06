using CORE.Scripts;

namespace Scenes._50_Minigames._65_MonsterTower.Scrips.MTGameModes
{
    public interface IMTGameMode : IGenericGameMode
    {
        /// <summary>
        /// Will be called by the TowerManager to create a brick with the correct answer
        /// </summary>
        /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetCorrectAnswer(string str, TowerManager manager);

        /// <summary>
        /// Will be called by the TowerManager to create a brick with an (usually random) incorrect answer
        /// </summary>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void SetWrongAnswer(TowerManager manager,string correctAnswer);

        /// <summary>
        /// Set's the answer key, which will tell the player which brick is correct. Uses the opposite medium of SetCorrectAnswer
        /// </summary>
        /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
        /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
        public void GetDisplayAnswer(string str, TowerManager manager);

        /// <summary>
        /// create a series of answers
        /// </summary>
        /// <param name="count">number of answers to create</param>
        /// <returns>Returns a set of answers strings to be used by the towerManager</returns>
        public string[] GenerateAnswers(int count);

        /// <summary>
        /// will pick the correct prefab from prefabs in the TowerManager, then set that to be the TowerManagers AnswerHolderPrefab
        /// </summary>
        /// <param name="manager">references back to the towermanager so it can pick one of the prefabs and use it for the answerholder</param>
        public void SetAnswerPrefab(TowerManager manager);
    }
}