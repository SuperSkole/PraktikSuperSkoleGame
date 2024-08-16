using UnityEngine;

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

        // Prepare the preview object by applying the preview material.
        PreparePreview(previewObject);

        // Prepare the cursor (grid indicator) based on the size of the object.
        PrepareCursor(size);

        // Show the cell indicator.
        cellIndicator.SetActive(true);
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
            isbuildingSystemEnabled=true;
        else isbuildingSystemEnabled=false;
     
        uiBuilding.SetActive(isbuildingSystemEnabled);

    }

    // Updates the position of the preview and cursor, and applies visual feedback based on placement validity.
    public void UpdatePosition(Vector3 position, bool validity)
    {
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
