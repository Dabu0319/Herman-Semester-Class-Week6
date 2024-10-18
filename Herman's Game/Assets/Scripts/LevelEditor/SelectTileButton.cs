using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Random,
    Eraser,
    Basic,

}


public class SelectTileButton : MonoBehaviour
{
    public Material materialToSelect;
    public float costToSelect;
    private TileSelector tileSelector;
    
    public TileType tileType = TileType.Basic;
   //public bool randomTile = false;

    void Start()
    {
        tileSelector = FindObjectOfType<TileSelector>();  
    }

    void OnMouseDown()
    {
        // When the player clicks the button,
        // set the selected material and cost

        switch (tileType)
        {
            case TileType.Random:
                tileSelector.tileType = TileType.Random;
                break;
            case TileType.Eraser:
                //tileSelector.SelectEraser();
                tileSelector.tileType = TileType.Eraser;
                break;
            case TileType.Basic:
                tileSelector.SelectTile(materialToSelect, costToSelect);
                Debug.Log("Selected: " + materialToSelect.name + "Cost: " + costToSelect);
                //tileSelector.randomSelected = false;
                tileSelector.tileType = TileType.Basic;
                break;
        }

        
        
        
    }
}
