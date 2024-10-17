using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public bool isSelecting = false;
    public Material selectedMaterial;  
    public float selectedCost;         

    public void SelectTile(Material newMaterial, float newCost)
    {
        selectedMaterial = newMaterial;
        selectedCost = newCost;
        Debug.Log("Selected material: " + newMaterial.name + " with cost: " + newCost);
        
        isSelecting = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            UnselectTile();
        }
    }
    
    
    public void UnselectTile()
    {
        if (!isSelecting)
        {
            return;
        }
        
        selectedMaterial = null;
        selectedCost = 0;
        isSelecting = false;
    }
}
