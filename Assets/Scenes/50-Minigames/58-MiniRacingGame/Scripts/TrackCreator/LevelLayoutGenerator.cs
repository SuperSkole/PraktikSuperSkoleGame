using System.Collections.Generic;
using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class LevelLayoutGenerator : MonoBehaviour
    {
        public LevelChunkdata[] levelTrackChunkData;
        public LevelChunkdata[] levelCheckpointChunkData;
        public LevelChunkdata[] levelBillboardChunkData;
        public LevelChunkdata firstChunk;

        [SerializeField]
        private RacingCore racingCore;

        private LevelChunkdata previousChunk;

        public Vector3 spawnOrigin;

        private Vector3 spawnposition;
        public int chunksToSpawn = 10;

        [SerializeField]
        private int chunksToCheckpoint = 5;
        [SerializeField]
        private int chunksToBillBoard = 10;
        private int chunksPassed = 0;
        /// <summary>
        /// Spawn new chunks of the map when called
        /// </summary>
        private void OnEnable()
        {
            {
                TriggerExit.OnChunkExited += PickAndSpawnChunk;
            }
        }

        private void OnDisable()
        {
            TriggerExit.OnChunkExited -= PickAndSpawnChunk;
        }

        /// <summary>
        /// Sets up the random with a seed, then begin spawning chunks
        /// </summary>
        private void Start()
        {
            Random.InitState(5000);
            previousChunk = firstChunk;

            for (int i = 0; i < chunksToSpawn; i++)
            {
                PickAndSpawnChunk();
            }
        }
        /// <summary>
        /// Picks the next chunk to be spawned, based on possible directions
        /// </summary>
        private LevelChunkdata PicknextChunk()
        {
            List<LevelChunkdata> allowedChunkList = new();
            LevelChunkdata nextChunk = null;

            LevelChunkdata.direction nextRequiredDirection = LevelChunkdata.direction.North;

            switch (previousChunk.exitDirection)
            {
                case LevelChunkdata.direction.North:
                    nextRequiredDirection = LevelChunkdata.direction.South;
                    spawnposition = spawnposition + new Vector3(0, 0, previousChunk.chunkSize.y);

                    break;
                case LevelChunkdata.direction.East:
                    nextRequiredDirection = LevelChunkdata.direction.West;
                    spawnposition = spawnposition + new Vector3(previousChunk.chunkSize.x, 0, 0);

                    break;
                case LevelChunkdata.direction.South:
                    nextRequiredDirection = LevelChunkdata.direction.North;
                    spawnposition = spawnposition + new Vector3(0, 0, -previousChunk.chunkSize.y);

                    break;
                case LevelChunkdata.direction.West:
                    nextRequiredDirection = LevelChunkdata.direction.East;
                    spawnposition = spawnposition + new Vector3(-previousChunk.chunkSize.x, 0, 0);

                    break;

                default:
                    break;
            }

            if (chunksPassed % chunksToBillBoard == 1)
            {
                for (int i = 0; i < levelBillboardChunkData.Length; i++)
                {
                    if (levelBillboardChunkData[i].entryDirection == nextRequiredDirection)
                    {
                        allowedChunkList.Add(levelBillboardChunkData[i]);
                    }
                }
            }
            // TODO: Implement checkpoints, later
            //else if (chunksPassed % chunksToCheckpoint == 0 && !(chunksPassed % chunksToBillBoard == 0))
            //{
            //    for (int i = 0; i < levelCheckpointChunkData.Length; i++)
            //    {
            //        if (levelCheckpointChunkData[i].entryDirection == nextRequiredDirection)
            //        {
            //            allowedChunkList.Add(levelCheckpointChunkData[i]);
            //        }
            //    }
            //}
            else
            {
                for (int i = 0; i < levelTrackChunkData.Length; i++)
                {
                    if (levelTrackChunkData[i].entryDirection == nextRequiredDirection)
                    {
                        allowedChunkList.Add(levelTrackChunkData[i]);
                    }
                }

            }

            nextChunk = allowedChunkList[Random.Range(0, allowedChunkList.Count)];

            return nextChunk;
        }
        /// <summary>
        /// Instantiates the chunk to spawn
        /// </summary>
        private void PickAndSpawnChunk()
        {
            LevelChunkdata chunkToSpawn = PicknextChunk();

            GameObject objectFromChunk = chunkToSpawn.levelChunks[Random.Range(0, chunkToSpawn.levelChunks.Length)];
            previousChunk = chunkToSpawn;
            Instantiate(objectFromChunk, spawnposition + spawnOrigin, Quaternion.identity);
            chunksPassed++;
        }
        /// <summary>
        /// To be used to update the origins when the player is moved by the floatingorigin script
        /// </summary>
        /// <param name="originDelta"></param>
        public void UpdateSpawnOrigin(Vector3 originDelta)
        {
            spawnOrigin = spawnOrigin + originDelta;
        }
    }
}
