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
        // When the player clicks the button,
        // set the selected material and cost
        tileSelector.SelectTile(materialToSelect, costToSelect);
        Debug.Log("Selected: " + materialToSelect.name + "Cost: " + costToSelect);
    }
}
