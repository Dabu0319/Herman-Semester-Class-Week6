using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public Material selectedMaterial;  
    public float selectedCost;         

    public void SelectTile(Material newMaterial, float newCost)
    {
        selectedMaterial = newMaterial;
        selectedCost = newCost;
        Debug.Log("Selected material: " + newMaterial.name + " with cost: " + newCost);
    }
}
