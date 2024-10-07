using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes._11_PlayerHouseScene.script.HouseScripts
{
    public class PreviewSystem : MonoBehaviour
    {
        [SerializeField] private float previewYOffset = 0.06f;
        [SerializeField] private GameObject cellIndicator;
        private GameObject previewObject;
        [SerializeField] private Material previewMaterialsPrefab;
        private Material previewMaterialInstance;
        private Renderer cellIndicatorRenderer;

        private bool isbuildingSystemEnabled;
        [SerializeField] private GameObject uiBuilding;
        private Vector3 OldPosition;

        #region Dictionaries
        Dictionary<string, Vector3> defaultPositions = new Dictionary<string, Vector3>()
    {
        { "SquareRugParent(Clone)", new Vector3(0.5f, 0f, 0.5f) },
        { "ChairParent(Clone)", new Vector3(0.5f, 0f, 0.5f) },
        { "RedSofaChairParent(Clone)", new Vector3(0.5f, 0f, 0.5f) },
        { "RedSofaSingleParent(Clone)", new Vector3(0.5f, 0f, 0.5f) },
        { "DoubleBedParent(Clone)", new Vector3(1f, 0f, 1f) }
    };

        // Rotation-specific mappings for each object
        Dictionary<float, Vector3> tablePositions = new Dictionary<float, Vector3>()
    {
        { 0f, new Vector3(0.2f, 0, 0.1f) },
        { 90f, new Vector3(0.1f, 0, 1.75f) },
        { 180f, new Vector3(1.75f, 0, 0.9f) },
        { 270f, new Vector3(0.9f, 0, 0.2f) }
    };

        Dictionary<float, Vector3> redSofaDoublePositions = new Dictionary<float, Vector3>()
    {
        { 0f, new Vector3(0.25f, 0, 0.25f) },
        { 90f, new Vector3(0.25f, 0, 1.75f) },
        { 180f, new Vector3(1.75f, 0, 0.75f) },
        { 270f, new Vector3(0.8f, 0, 0.3f) }
    };

        Dictionary<float, Vector3> doubleDeckerBedPositions = new Dictionary<float, Vector3>()
    {
        { 0f, new Vector3(0.07f, 0, 0.2f) },
        { 90f, new Vector3(0.2f, 0, 0.94f) },
        { 180f, new Vector3(0.95f, 0, 1.75f) },
        { 270f, new Vector3(1.8f, 0, 0.05f) }
    };

        Dictionary<float, Vector3> singleBedPositions = new Dictionary<float, Vector3>()
    {
        { 0f, new Vector3(0.07f, 0, 0.2f) },
        { 90f, new Vector3(0.2f, 0, 0.9f) },
        { 180f, new Vector3(0.95f, 0, 1.8f) },
        { 270f, new Vector3(1.8f, 0, 0.07f) }
    };
        Dictionary<float, Vector3> StandardWallparent = new Dictionary<float, Vector3>()
    {
        { 0f, new Vector3(0f, 0f, 0f) },
        { 90f, new Vector3(0f, 0f, 2f) },
        { 180f, new Vector3(2f, 0f, 1f) },
        { 270f, new Vector3(1f, 0f, 0f) }
    };
        Dictionary<float, Vector3> WallWindowParent = new Dictionary<float, Vector3>()
    {
        { 0f, new Vector3(0f, 0f, 0f) },
        { 90f, new Vector3(0f, 0f, 2f) },
        { 180f, new Vector3(2f, 0f, 1f) },
        { 270f, new Vector3(1f, 0f, 0f) }
    };
        Dictionary<float, Vector3> IntranceDoorParent = new Dictionary<float, Vector3>()
    {
        { 0f, new Vector3(0f, 0f, 0f) },
        { 90f, new Vector3(0f, 0f, 2f) },
        { 180f, new Vector3(2f, 0f, 1f) },
        { 270f, new Vector3(1f, 0f, 0f) }
    };


        #endregion
        private void Start()
        {
            // Create an instance of the preview material from the prefab.
            previewMaterialInstance = new Material(previewMaterialsPrefab);

            // Initially hide the cell indicator since there's no object being placed.
            cellIndicator.SetActive(false);

            // Get the renderer component of the cell indicator for later use.
            cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
        }

        // Starts showing the placement preview for a given prefab with a specific size.
        public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
        {
            // Instantiate the preview object from the provided prefab.
            previewObject = Instantiate(prefab);

            previewObject.transform.position += ReturnLocationOfGameObject(previewObject.name, previewObject);

            // Prepare the preview object by applying the preview material.
            PreparePreview(previewObject);

            // Prepare the cursor (grid indicator) based on the size of the object.
            PrepareCursor(size);

            // Show the cell indicator.
            cellIndicator.SetActive(true);
        }


        public Vector3 ReturnLocationOfGameObject(string name, GameObject item)
        {
            // Check if the object name exists in the default positions dictionary
            if (defaultPositions.TryGetValue(name, out Vector3 defaultPosition))
            {
                return defaultPosition;
            }

            // Get the rounded rotation of the item (for better accuracy)
            float rotationY = Mathf.Round(item.transform.rotation.eulerAngles.y);

            // Handle rotation-specific objects
            switch (name)
            {
                case "TableParent 1(Clone)":
                    return tablePositions.ContainsKey(rotationY) ? tablePositions[rotationY] : Vector3.zero;
                case "RedSofaDoubleParent(Clone)":
                    return redSofaDoublePositions.ContainsKey(rotationY) ? redSofaDoublePositions[rotationY] : Vector3.zero;
                case "DoubleDeckerBedParent(Clone)":
                    return doubleDeckerBedPositions.ContainsKey(rotationY) ? doubleDeckerBedPositions[rotationY] : Vector3.zero;
                case "SingleBedParent(Clone)":
                    return singleBedPositions.ContainsKey(rotationY) ? singleBedPositions[rotationY] : Vector3.zero;

                //None Placeables Only for the house walls
                case "StandardWallparent(Clone)":
                    return StandardWallparent.ContainsKey(rotationY) ? StandardWallparent[rotationY] : Vector3.zero;
                case "WallWindowParent(Clone)":
                    return WallWindowParent.ContainsKey(rotationY) ? WallWindowParent[rotationY] : Vector3.zero;
                case "IntranceDoorParent(Clone)":
                    return IntranceDoorParent.ContainsKey(rotationY) ? IntranceDoorParent[rotationY] : Vector3.zero;
                    
                default:
                    return Vector3.zero;
            }
        }
        public void RotateItem(int degree, PlacementState state)
        {
            // Rotate the object around the Y-axis by the specified degree
            //*= applies this rotation relative to the current orientation, ensuring consistent and smooth rotations.
            previewObject.transform.rotation *= Quaternion.Euler(0, degree, 0);

            var xtmp = state.SizeCopy.x;
            var ytmp = state.SizeCopy.y;
            var tmp = xtmp;
            xtmp = ytmp;
            ytmp = tmp;
            state.SizeCopy = new Vector2Int(xtmp, ytmp);
            PrepareCursor(state.SizeCopy);
            MovePreview(OldPosition);

        }
        public quaternion ReturnItemRotation()
        {
            return previewObject.transform.rotation;
        }
        public float ReturnYEulerAngelsOnPreviewItem()
        {
            return previewObject.transform.rotation.eulerAngles.y;
        }
        // Prepares the cursor's visual appearance based on the size of the object to be placed.
        private void PrepareCursor(Vector2Int size)
        {
            if (size.x > 0 || size.y > 0)
            {
                // Adjust the scale of the cell indicator to match the object's size.
                cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);

                // Adjust the texture scale of the material to match the object's size.
                cellIndicatorRenderer.material.mainTextureScale = size;
            }
        }

        // Prepares the preview object by applying the preview material to all its renderers.
        private void PreparePreview(GameObject previewObject)
        {
            // Get all renderer components in the preview object (including children).
            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

            // Apply the preview material to each renderer.
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = previewMaterialInstance;
                }
                renderer.materials = materials;
            }
        }

        // Stops showing the preview by hiding the indicator and destroying the preview object.
        public void StopShowingPreview()
        {
            // Hide the cell indicator.
            cellIndicator.SetActive(false);

            // If a preview object exists, destroy it.
            if (previewObject != null)
            {
                Destroy(previewObject);
            }
        }

        public void DisableBuildingSystem()
        {
            isbuildingSystemEnabled = false;
            uiBuilding.SetActive(false);
        }
        public void EnableBuildingSystem()
        {
            if (!isbuildingSystemEnabled)
                isbuildingSystemEnabled = true;
            else isbuildingSystemEnabled = false;

            uiBuilding.SetActive(isbuildingSystemEnabled);

        }

        // Updates the position of the preview and cursor, and applies visual feedback based on placement validity.
        public void UpdatePosition(Vector3 position, bool validity)
        {
            OldPosition = position;

            if (isbuildingSystemEnabled) { }
            if (previewObject != null)
            {
                // Move the preview object to the specified position.
                MovePreview(position);

                // Change the color of the preview material based on placement validity.
                ApplyFeedbackToPreview(validity);
            }

            // Move the cursor to the specified position.
            MoveCursor(position);

            // Change the color of the cursor material based on placement validity.
            ApplyFeedbackToCursor(validity);
        }

        // Applies visual feedback to the preview object based on whether the placement is valid.
        private void ApplyFeedbackToPreview(bool validity)
        {
            // Set the color to white if valid, red if invalid, with 50% transparency.
            Color c = validity ? Color.white : Color.red;
            c.a = 0.5f;
            previewMaterialInstance.color = c;
        }

        // Applies visual feedback to the cursor based on whether the placement is valid.
        private void ApplyFeedbackToCursor(bool validity)
        {
            // Set the color to white if valid, red if invalid, with 50% transparency.
            Color c = validity ? Color.white : Color.red;
            c.a = 0.5f;
            cellIndicatorRenderer.material.color = c;
        }

        // Moves the cursor (grid indicator) to the specified position.
        private void MoveCursor(Vector3 position)
        {
            cellIndicator.transform.position = position;
        }

        // Moves the preview object to the specified position, applying a vertical offset.
        private void MovePreview(Vector3 position)
        {
            previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
            previewObject.transform.position += ReturnLocationOfGameObject(previewObject.name, previewObject);

        }

        // Starts showing a removal preview, highlighting the grid cell in red.
        internal void StartShowingRemovePreview()
        {
            // Show the cell indicator and set its size to 1x1.
            cellIndicator.SetActive(true);
            PrepareCursor(Vector2Int.one);

            // Apply the removal feedback (red color) to the cursor.
            ApplyFeedbackToCursor(false);
        }
    }
}
