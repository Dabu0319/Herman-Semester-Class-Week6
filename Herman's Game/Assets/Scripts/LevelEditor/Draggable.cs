using UnityEngine;

public class Draggable : MonoBehaviour
{

    // Mouse drag function script 
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private GridScript gridScript;
    private TileSelector tileSelector;  
    private float offsetX;
    private float offsetY;

    void Start()
    {
        mainCamera = Camera.main;
        gridScript = FindObjectOfType<GridScript>();
        tileSelector = FindObjectOfType<TileSelector>();  // Get TileSelector
        offsetX = (gridScript.gridWidth * -gridScript.spacing) / 2f;
        offsetY = (gridScript.gridHeight * gridScript.spacing) / 2f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Detecting clicked 2D Sprite using Physics2D.Raycast
            if (hit.collider != null && hit.transform == transform)
            {
                isDragging = true;

                offset = GetMouseWorldPosition() - transform.position;
            }
        }

        if (isDragging)
        {
            Vector3 newMousePos = GetMouseWorldPosition() - offset;
            transform.position = newMousePos;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            // Snap the object's position to the nearest grid when releasing the mouse button
            Vector3 snappedPos = SnapToGrid(transform.position);
            transform.position = snappedPos;

            // Update the start / goal position in GridScript
            UpdateGridScriptPosition(snappedPos);
        }

        // Detect if the grid tile is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // If tile is clicked
            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                GameObject clickedTile = hit.collider.gameObject;

                // Get tile position
                Vector3 tilePosition = clickedTile.transform.position;
                int gridX = Mathf.RoundToInt((tilePosition.x - offsetX) / gridScript.spacing);
                int gridY = Mathf.RoundToInt((offsetY - tilePosition.y) / gridScript.spacing);

                // Use TileSelector to update the tile's material and cost
                gridScript.UpdateTile(gridX, gridY, tileSelector.selectedMaterial, tileSelector.selectedCost);
            }
        }
    }

    private void UpdateGridScriptPosition(Vector3 snappedPos)
    {
        int gridX = Mathf.RoundToInt((snappedPos.x - offsetX) / gridScript.spacing);
        int gridY = Mathf.RoundToInt((offsetY - snappedPos.y) / gridScript.spacing);

        if (gameObject == gridScript.startSprite)
        {
            gridScript.start = new Vector3(gridX, gridY);
        }
        else if (gameObject == gridScript.goalSprite)
        {
            gridScript.goal = new Vector3(gridX, gridY);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }


    // Align and snap with grid
    private Vector3 SnapToGrid(Vector3 position)
    {
        float offsetX = (gridScript.gridWidth * -gridScript.spacing) / 2f;
        float offsetY = (gridScript.gridHeight * gridScript.spacing) / 2f;

        float relativeX = position.x - offsetX;
        float relativeY = offsetY - position.y;

        int gridX = Mathf.RoundToInt(relativeX / gridScript.spacing);
        int gridY = Mathf.RoundToInt(relativeY / gridScript.spacing);

        gridX = Mathf.Clamp(gridX, 0, gridScript.gridWidth - 1);
        gridY = Mathf.Clamp(gridY, 0, gridScript.gridHeight - 1);

        float snapX = offsetX + gridX * gridScript.spacing;
        float snapY = offsetY - gridY * gridScript.spacing;

        return new Vector3(snapX, snapY, position.z);
    }
}