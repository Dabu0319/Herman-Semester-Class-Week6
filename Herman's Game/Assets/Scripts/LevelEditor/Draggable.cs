using UnityEngine;

public class Draggable : MonoBehaviour
{

    //鼠标拖动功能脚本 
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
        tileSelector = FindObjectOfType<TileSelector>();  // 获取 TileSelector
        offsetX = (gridScript.gridWidth * -gridScript.spacing) / 2f;
        offsetY = (gridScript.gridHeight * gridScript.spacing) / 2f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // 使用 Physics2D.Raycast 检测点击的 2D Sprite
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

            // 在松开鼠标时，将物体位置对齐到最近的网格
            Vector3 snappedPos = SnapToGrid(transform.position);
            transform.position = snappedPos;

            // 更新 GridScript 中的 start 或 goal 位置
            UpdateGridScriptPosition(snappedPos);
        }

        // 检测网格 tile 是否被点击
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // 如果点击了 tile
            if (hit.collider != null && hit.collider.CompareTag("Tile"))
            {
                GameObject clickedTile = hit.collider.gameObject;

                // 获取 tile 的位置
                Vector3 tilePosition = clickedTile.transform.position;
                int gridX = Mathf.RoundToInt((tilePosition.x - offsetX) / gridScript.spacing);
                int gridY = Mathf.RoundToInt((offsetY - tilePosition.y) / gridScript.spacing);

                // 使用 TileSelector 更新该 tile 的材质和cost
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


    //和grid对齐
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