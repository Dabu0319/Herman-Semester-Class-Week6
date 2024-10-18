using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public Material selectedMaterial;  
    public float selectedCost;
    public List<GameObject> previewPrefabs; 
    private GameObject currentPreview;  

    public TileType tileType = TileType.Basic;

    private GridScript gridScript; // Reference to GridScript

    private void Start()
    {
        gridScript = FindObjectOfType<GridScript>();  // Get the GridScript
    }

    void Update()
    {
        // If the preview is active, move it with the mouse and snap to the nearest grid
        if (currentPreview != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Ensure it's placed on the correct plane

            // Find the nearest grid position and snap the preview to that position
            Vector3 closestGridPos = GetClosestGridPosition(mousePos);
            currentPreview.transform.position = closestGridPos;
        }

        // Cancel preview if right mouse button is clicked
        if (Input.GetMouseButton(1))
        {
            selectedMaterial = null;
            selectedCost = 0;
            StopTilePreview();
        }
    }

    // Method to find the nearest grid position based on the mouse position
    private Vector3 GetClosestGridPosition(Vector3 mousePosition)
    {
        // Get grid parameters from GridScript
        int gridWidth = gridScript.gridWidth;
        int gridHeight = gridScript.gridHeight;
        float spacing = gridScript.spacing;

        // Calculate offsets to position the grid in the center
        float offsetX = (gridWidth * -spacing) / 2f;
        float offsetY = (gridHeight * spacing) / 2f;

        // Calculate the closest grid X and Y based on mouse position
        int closestX = Mathf.RoundToInt((mousePosition.x - offsetX) / spacing);
        int closestY = Mathf.RoundToInt((offsetY - mousePosition.y) / spacing);

        // Clamp to ensure the selected grid is within bounds
        closestX = Mathf.Clamp(closestX, 0, gridWidth - 1);
        closestY = Mathf.Clamp(closestY, 0, gridHeight - 1);

        // Convert the closest grid indices back to world position
        Vector3 closestGridPos = new Vector3(offsetX + closestX * spacing, offsetY - closestY * spacing, 0);
        return closestGridPos;
    }

    public void StartTilePreview(string previewName)
    {
        StopTilePreview();  // Close any existing preview

        // Instantiate the correct preview prefab
        foreach (GameObject prefab in previewPrefabs)
        {
            if (prefab.name == previewName)
            {
                currentPreview = Instantiate(prefab);
                currentPreview.SetActive(true);  
                Debug.Log("Preview started: " + previewName);
                return;
            }
        }

        Debug.LogWarning("No preview prefab found for: " + previewName);
    }

    public void StopTilePreview()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);  
            currentPreview = null;
        }
    }

    public void SelectTile(Material newMaterial, float newCost)
    {
        selectedMaterial = newMaterial;
        selectedCost = newCost;
        Debug.Log("Selected material: " + newMaterial.name + " with cost: " + newCost);
    }
}
