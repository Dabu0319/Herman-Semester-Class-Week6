using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTileButton : MonoBehaviour
{
    public Material materialToSelect;
    public float costToSelect;
    private TileSelector tileSelector;

    void Start()
    {
        tileSelector = FindObjectOfType<TileSelector>();  
    }

    void OnMouseDown()
    {
        // 当玩家点击按钮时，设置选中的材质和成本
        tileSelector.SelectTile(materialToSelect, costToSelect);
        Debug.Log("Selected: " + materialToSelect.name + "Cost: " + costToSelect);
    }
}
