using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public Material selectedMaterial;  
    public float selectedCost;
    //private GridScript _gridScript;
    //public bool randomSelected = false;

    public TileType tileType = TileType.Basic;
    
    public void SelectTile(Material newMaterial, float newCost)
    {
        selectedMaterial = newMaterial;
        selectedCost = newCost;
        Debug.Log("Selected material: " + newMaterial.name + " with cost: " + newCost);
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            selectedMaterial = null;
            selectedCost = 0;
        }
    }
}
