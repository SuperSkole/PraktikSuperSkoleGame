using System;
using System.Reflection;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace LoadSave
{
    /// <summary>
    /// Responsible for converting between PlayerData and SaveDataDTO using reflection to minimize manual field mapping.
    /// </summary>
    public class DataConverter
    {
        /// <summary>
        /// Converts PlayerData to SaveDataDTO for saving to the cloud.
        /// Uses reflection to copy matching fields dynamically.
        /// </summary>
        /// <param name="playerData">The PlayerData object to convert.</param>
        /// <returns>A SaveDataDTO object representing the player's data.</returns>
        public SaveDataDTO ConvertToDTO(PlayerData playerData)
        {
            var dto = new SaveDataDTO();
            // Use reflection to copy matching fields
            CopyFields(playerData, dto);
            
            return dto;
        }

        /// <summary>
        /// Converts SaveDataDTO back to PlayerData for use in the game.
        /// Uses reflection to copy matching fields dynamically.
        /// </summary>
        /// <param name="dto">The SaveDataDTO object to convert.</param>
        /// <returns>A PlayerData object populated with the data from the DTO.</returns>
        public PlayerData ConvertToPlayerData(SaveDataDTO dto)
        {
            // Create and initialize the PlayerData component
            PlayerData playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();
            
            // Use reflection to copy matching fields
            CopyFields(dto, playerData);
            
            return playerData;
        }

        /// <summary>
        /// Copies matching fields between source and destination objects using reflection.
        /// Only fields with the same name and type are copied.
        /// </summary>
        /// <param name="source">The source object to copy data from.</param>
        /// <param name="destination">The destination object to copy data to.</param>
        private void CopyFields(object source, object destination)
        {
            // Get the type (class definition) of the source object
            Type sourceType = source.GetType();
            // Get the type (class definition) of the destination object
            Type destType = destination.GetType();

            // Get all the public instance fields (variables) of the destination object
            FieldInfo[] destFields = destType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            // Loop through each field in the destination object
            foreach (var destField in destFields)
            {
                // Try to find a field with the same name in the source object
                FieldInfo sourceField = sourceType.GetField(destField.Name, BindingFlags.Public | BindingFlags.Instance);

                // Check if the source field exists
                if (sourceField != null)
                {
                    // Check if the types of the source and destination fields match
                    if (sourceField.FieldType == destField.FieldType)
                    {
                        // Get the value from the source field
                        var value = sourceField.GetValue(source);
                        // Set that value to the corresponding destination field
                        destField.SetValue(destination, value);
                    }
                    else if (sourceField.FieldType == typeof(SerializablePlayerPosition) && destField.FieldType == typeof(Vector3))
                    {
                        // Handle conversion from SerializablePlayerPosition to Vector3
                        SerializablePlayerPosition serializablePosition = (SerializablePlayerPosition)sourceField.GetValue(source);
                        var vector = serializablePosition.GetVector3();
                        destField.SetValue(destination, vector);
                    }
                    else if (sourceField.FieldType == typeof(Vector3) && destField.FieldType == typeof(SerializablePlayerPosition))
                    {
                        // Handle conversion from Vector3 to SerializablePlayerPosition
                        Vector3 vector = (Vector3)sourceField.GetValue(source);
                        var serializablePosition = new SerializablePlayerPosition(vector);
                        destField.SetValue(destination, serializablePosition);
                    }
                }
            }
        }
    }
}
