using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public class ObjectPlacer : MonoBehaviour
    {
        // List to store references to all the objects that have been placed.
        [SerializeField] private List<GameObject> placedGameObjects = new List<GameObject>();
        [SerializeField] private GameObject parent;

        [SerializeField] PreviewSystem previewSystem;
        

        public List<GameObject> PlacedGameObjects {  get { return placedGameObjects; } set { placedGameObjects = value; } }
        // Method to place an object at the specified position.
        // Takes a prefab and a position in the world as arguments.
        // Returns the index of the placed object in the list.
        public int PlaceObject(GameObject prefab, Vector3 pos, quaternion rotation)
        {
            // Instantiate a new object from the provided prefab.
            GameObject newObject = Instantiate(prefab,parent.transform);


            // Set the position of the new object to the specified position.
            newObject.transform.position = pos;
            newObject.transform.rotation *= rotation;

            //This nudges the placed GO to the middle of the click gridplace,
            //If new models are used or the grid is smaller change this accordingly.
            newObject.transform.position += previewSystem.ReturnLocationOfGameObject(newObject.name, newObject);

            // Add the newly placed object to the list of placed objects.
            placedGameObjects.Add(newObject);

            // Return the index of the placed object, which is the last element in the list.
            return placedGameObjects.Count - 1;
        }

        // Method to remove an object at the specified index in the list.
        // Takes the index of the object to be removed as an argument.
        internal void RemoveObjectAt(int gameObjectIndex)
        {
            // Check if the index is valid and if the object at that index is not null.
            if (placedGameObjects.Count <= gameObjectIndex
                || placedGameObjects[gameObjectIndex] == null)
            {
                return;  // If the index is invalid or the object is null, exit the method.
            }

            // Destroy the game object at the specified index.
            Destroy(placedGameObjects[gameObjectIndex]);

            // Set the reference in the list to null to indicate the object has been removed.
            placedGameObjects[gameObjectIndex] = null;
        }
        
    }
}

