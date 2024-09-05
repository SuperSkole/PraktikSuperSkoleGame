using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    /// <summary>
    /// Chunks that determine what can be procedurally generated with
    /// </summary>
    [CreateAssetMenu(menuName = "LevelChunkData")]
    public class LevelChunkdata : ScriptableObject
    {
        public enum direction
        {
            North, East, South, West
        }

        public Vector2 chunkSize = new(10f, 10f);

        public GameObject[] levelChunks;
        public direction entryDirection;
        public direction exitDirection;
    }
}
