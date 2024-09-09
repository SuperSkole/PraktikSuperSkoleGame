using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scenes._50_Minigames._58_MiniRacingGame.Scripts
{
    public class LevelLayoutGenerator : MonoBehaviour
    {
        public LevelChunkdata[] levelTrackChunkData;
        public LevelChunkdata[] levelCheckpointChunkData;
        public LevelChunkdata[] levelBillboardChunkData;
        public LevelChunkdata[] levelFinaleChunkData;
        public LevelChunkdata firstChunk;

        [SerializeField]
        public TextMeshProUGUI mapSeedText;
        [SerializeField]
        private RacingCore racingCore;

        [SerializeField]
        private string mapSeed;

        public string mapSeedSuggestion = "";

        private LevelChunkdata previousChunk;

        public Vector3 spawnOrigin;
        public bool finalStretch = false;
        private bool finaleMade = false;

        private Vector3 spawnposition;
        public int chunksToSpawn = 10;

        [SerializeField]
        private int chunksToCheckpoint = 5;
        [SerializeField]
        private int chunksToBillBoard = 10;
        private int chunksPassed = 0;
        /// <summary>
        /// Sets up the map and random seed when the track is enabled
        /// </summary>
        private void OnEnable()
        {
            {
                TriggerExit.OnChunkExited += PickAndSpawnChunk;
            }
            string mapInput = "";//mapSeedText.text; FIX: The seed code doesn't get from the input properly. It apparently receives unicode characters. No idea for a fix.
            if (mapInput is "" or " ")
            {
                mapSeedText.text = mapSeedSuggestion;
            }
            mapSeed = mapSeedText.text;
            int finalSeed = 0;
            string result = "";
            foreach (char c in mapSeed)
            {
                result += GetIndexInAlphabet(c).ToString();
            }
            finalSeed = Convert.ToInt32(result) * 3;
            UnityEngine.Random.InitState(finalSeed);
            previousChunk = firstChunk;

            for (int i = 0; i < chunksToSpawn; i++)
            {
                PickAndSpawnChunk();
            }
            racingCore.UpdateBillBoard();
        }

        private void OnDisable()
        {
            TriggerExit.OnChunkExited -= PickAndSpawnChunk;
        }

        /// <summary>
        /// Converts a latin character to the corresponding letter's index in the standard Latin alphabet
        /// </summary>
        /// <param name="value">An upper- or lower-case Latin character</param>
        /// <returns>The 0-based index of the letter in the Latin alphabet</returns>
        private static int GetIndexInAlphabet(char value)
        {
            // Uses the uppercase character unicode code point. 'A' = U+0042 = 65, 'Z' = U+005A = 90
            char upper = char.ToUpper(value);
            if (upper is < 'A' or > 'Z')
            {
                return 0;
            }

            return upper - 'A';
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
            if (finalStretch)
            {
                for (int i = 0; i < levelFinaleChunkData.Length; i++)
                {
                    if (levelFinaleChunkData[i].entryDirection == nextRequiredDirection)
                    {
                        allowedChunkList.Add(levelFinaleChunkData[i]);
                        finaleMade = true;
                    }
                }
            }
            else if (chunksPassed % chunksToBillBoard == 1)
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

            nextChunk = allowedChunkList[UnityEngine.Random.Range(0, allowedChunkList.Count)];

            return nextChunk;
        }
        /// <summary>
        /// Instantiates the chunk to spawn
        /// </summary>
        private void PickAndSpawnChunk()
        {
            if (finaleMade)
                return;
            LevelChunkdata chunkToSpawn = PicknextChunk();

            GameObject objectFromChunk = chunkToSpawn.levelChunks[UnityEngine.Random.Range(0, chunkToSpawn.levelChunks.Length)];
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
