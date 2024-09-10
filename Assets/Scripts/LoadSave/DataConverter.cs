using Scenes._10_PlayerScene.Scripts;

namespace LoadSave
{
    /// <summary>
    /// Responsible for converting between PlayerData and SaveDataDTO.
    /// </summary>
    public class DataConverter
    {
        /// <summary>
        /// Converts PlayerData to SaveDataDTO for saving to the cloud.
        /// </summary>
        /// <param name="playerData">The PlayerData object to convert.</param>
        /// <returns>A SaveDataDTO object representing the player's data.</returns>
        public SaveDataDTO ConvertToDTO(PlayerData playerData)
        {
            return new SaveDataDTO
            {
                Username = playerData.Username,
                Savefile = playerData.Savefile,
                MonsterName = playerData.MonsterName,
                MonsterTypeID = playerData.MonsterTypeID,
                MonsterColor = playerData.MonsterColor,
                GoldAmount = playerData.CurrentGoldAmount,
                XPAmount = playerData.CurrentXPAmount,
                PlayerLevel = playerData.CurrentLevel,
                SavedPlayerStartPostion = new SerializablePlayerPosition(playerData.CurrentPosition),
                CollectedWords = playerData.CollectedWords,
                CollectedLetters = playerData.CollectedLetters,
                BoughtClothes = playerData.BoughtClothes,
                clothMid = playerData.ClothMid,
                clothTop = playerData.ClothTop
            };
        }

        /// <summary>
        /// Converts SaveDataDTO back to PlayerData for use in the game.
        /// </summary>
        /// <param name="dto">The SaveDataDTO object to convert.</param>
        /// <returns>A PlayerData object populated with the data from the DTO.</returns>
        public PlayerData ConvertToPlayerData(SaveDataDTO dto)
        {
            PlayerData playerData = PlayerManager.Instance.SpawnedPlayer.AddComponent<PlayerData>();
            playerData.Initialize(
                dto.Username,
                dto.MonsterName,
                dto.MonsterColor,
                dto.GoldAmount,
                dto.XPAmount,
                dto.PlayerLevel,
                dto.SavedPlayerStartPostion.GetVector3(),
                dto.clothMid,
                dto.clothTop);

            playerData.CollectedWords = dto.CollectedWords;
            playerData.CollectedLetters = dto.CollectedLetters;
            playerData.BoughtClothes = dto.BoughtClothes;

            return playerData;
        }
    }
}
