using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CORE;
using UnityEngine;

namespace LoadSave
{
    /// <summary>
    /// Responsible for converting between PlayerData and SaveDataDTO using reflection to minimize manual mapping.
    /// </summary>
    public class DataConverter
    {
        /// <summary>
        /// Converts PlayerData to SaveDataDTO for saving to the cloud.
        /// Uses reflection to copy matching properties dynamically.
        /// </summary>
        /// <param name="playerData">The PlayerData object to convert.</param>
        /// <returns>A SaveDataDTO object representing the player's data.</returns>
        public SaveDataDTO ConvertToDTO(PlayerData playerData)
        {
            var dto = new SaveDataDTO();
            // Use reflection to copy matching properties
            CopyProperties(playerData, dto);
            return dto;
        }

        /// <summary>
        /// Converts SaveDataDTO back to PlayerData for use in the game.
        /// Uses reflection to copy matching properties dynamically.
        /// </summary>
        /// <param name="dto">The SaveDataDTO object to convert.</param>
        /// <returns>A PlayerData object populated with the data from the DTO.</returns>
        public PlayerData ConvertToPlayerData(SaveDataDTO dto)
        {
            // Create and initialize the PlayerData component
            PlayerData playerData = GameManager.Instance.PlayerData;

            // Use reflection to copy matching properties
            CopyProperties(dto, playerData);

            return playerData;
        }

        /// <summary>
        /// Copies matching properties between source and destination objects using reflection.
        /// Only properties with the same name and type are copied.
        /// </summary>
        /// <param name="source">The source object to copy data from.</param>
        /// <param name="destination">The destination object to copy data to.</param>
        private void CopyProperties(object source, object destination)
        {
            // Get the type (class definition) of the source object
            Type sourceType = source.GetType();
            // Get the type (class definition) of the destination object
            Type destType = destination.GetType();

            // Get all the public instance properties of the destination object
            PropertyInfo[] destProperties = destType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //FieldInfo[] destFields = destType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var destProperty in destProperties)
            {
                // EO; Skip properties that are part of Unity base classes (like MonoBehaviour)
                if (destProperty.DeclaringType != destType)
                {
                    continue; 
                }

                // EO; Skip properties marked with the ExcludeFromSave attribute
                if (destProperty.IsDefined(typeof(ExcludeFromSaveAttribute), false))
                {
                    continue; 
                }

                // Try to find a property with the same name in the source object
                PropertyInfo sourceProperty = sourceType.GetProperty(destProperty.Name, BindingFlags.Public | BindingFlags.Instance);

                // Check if the source property exists, is readable, and the destination property is writable
                if (sourceProperty != null && sourceProperty.CanRead && destProperty.CanWrite)
                {
                    // Get the value from the source property
                    var value = sourceProperty.GetValue(source);
            
                    // Add null check for value before setting it on the destination
                    if (value != null)
                    {
                        // Check if the types of the source and destination properties match
                        if (sourceProperty.PropertyType == destProperty.PropertyType)
                        {
                            // Check if the property is a list (handle lists specifically)
                            if (typeof(IList).IsAssignableFrom(sourceProperty.PropertyType))
                            {
                                // Clear the destination list before copying to avoid duplication
                                IList destList = (IList)destProperty.GetValue(destination);
                                if (destList != null)
                                {
                                    destList.Clear();
                                }
                                
                                // Log to ensure lists are detected
                                //Debug.Log($"Copying list property: {destProperty.Name}");

                                // Copy the list
                                destProperty.SetValue(destination, value);

                                // Log the number of items being copied
                                IList sourceList = (IList)value;
                                //Debug.Log($"List {destProperty.Name} copied with {sourceList.Count} items.");
                            }

                            else
                            {
                                // Set the value to the corresponding destination property
                                destProperty.SetValue(destination, value);
                            }
                        }
                        // Handle conversion from SerializablePlayerPosition to Vector3
                        else if (sourceProperty.PropertyType == typeof(SerializablePlayerPosition) && destProperty.PropertyType == typeof(Vector3))
                        {
                            SerializablePlayerPosition serializablePosition = (SerializablePlayerPosition)value;
                            var vector = serializablePosition.GetVector3();
                            destProperty.SetValue(destination, vector);
                        }
                        // Handle conversion from Vector3 to SerializablePlayerPosition
                        else if (sourceProperty.PropertyType == typeof(Vector3) && destProperty.PropertyType == typeof(SerializablePlayerPosition))
                        {
                            Vector3 vector = (Vector3)value;
                            var serializablePosition = new SerializablePlayerPosition(vector);
                            destProperty.SetValue(destination, serializablePosition);
                        }
                        else
                        {
                            // Log type mismatch
                            Debug.LogWarning($"Type mismatch for property {destProperty.Name}. Source: {sourceProperty.PropertyType}, Destination: {destProperty.PropertyType}");
                        }
                    }
                    else
                    {
                        // Log null value
                        Debug.LogWarning($"Null value found for source property: {sourceProperty.Name}");
                    }
                }
                else
                {
                    // Log missing source property or no write access
                    Debug.LogWarning($"DataConverter: No matching source property found or destination property is not writable: {destProperty.Name}");
                    Debug.Log($"DataConverter: Trying to copy {destProperty.Name} from {source}. Did you forget private field, public property in playerdata or savedata?");
                }
            }
        }
    }
}