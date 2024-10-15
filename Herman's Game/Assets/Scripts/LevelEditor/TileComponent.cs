using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    // 这个组件在grid脚本中自动assign给每一个grid
   private GridScript gridScript;
    private int gridX, gridY;
    private Material currentMaterial;
    private float currentCost;
    private TileSelector tileSelector;

    public void Initialize(GridScript grid, int x, int y, Material material, float cost)
    {
        gridScript = grid;
        gridX = x;
        gridY = y;
        currentMaterial = material;
        currentCost = cost;

        // 初始化材质
        GetComponent<Renderer>().material = currentMaterial;
    }

    void Start()
    {
        tileSelector = FindObjectOfType<TileSelector>();  // 获取 TileSelector，用于选择材质和cost
    }

    void OnMouseDown()
    {
        // 检查是否有选中的材质和cost，没有就不执行替换操作
        if (tileSelector != null && tileSelector.selectedMaterial != null && tileSelector.selectedCost > 0)
        {
            // 更新 tile 的材质和cost
            gridScript.UpdateTile(gridX, gridY, tileSelector.selectedMaterial, tileSelector.selectedCost);

            GetComponent<Renderer>().material = tileSelector.selectedMaterial;
            currentCost = tileSelector.selectedCost;

            Debug.Log($"Tile at ({gridX}, {gridY}) updated with material: {tileSelector.selectedMaterial.name}, cost: {tileSelector.selectedCost}");
        }
    }
}

