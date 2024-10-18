using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileComponent : MonoBehaviour
{
    // This component is automatically assigned to each grid in the grid script
    private GridScript gridScript;
    private int gridX, gridY;
    private Material currentMaterial;
    private float currentCost;
    private TileSelector tileSelector;
    private bool isHolding = false;
    private bool isRandomized = false; 

    
    public void Initialize(GridScript grid, int x, int y, Material material, float cost)
    {
        gridScript = grid;
        gridX = x;
        gridY = y;
        currentMaterial = material;
        currentCost = cost;

        // Initialize material
        GetComponent<Renderer>().material = currentMaterial;
    }

    void Start()
    {
        tileSelector = FindObjectOfType<TileSelector>();  // Get TileSelector for material and cost
    }

    private void Update()
    {

        }
    

    public void OnMouseDown()
    {
        switch (tileSelector.tileType)
        {
            case TileType.Random:
                if (!isRandomized)
                {
                    int randomIndex = UnityEngine.Random.Range(0, gridScript.mats.Length);
                    Material randomMaterial = gridScript.mats[randomIndex];
                    float randomCost = gridScript.costs[randomIndex];
                    // Update the tile's material and cost
                    
                    UpdateTile(randomMaterial, randomCost);
                    isRandomized = true;
                }
                break;
            case TileType.Eraser:
                tileSelector.selectedMaterial = gridScript.mats[0];
                tileSelector.selectedCost = gridScript.costs[0];
                UpdateTile( tileSelector.selectedMaterial, tileSelector.selectedCost);
                
                break;
            case TileType.Basic:
                if (tileSelector != null && tileSelector.selectedMaterial != null && tileSelector.selectedCost > 0)
                {
                    // Update the tile's material and cost
                    UpdateTile(tileSelector.selectedMaterial, tileSelector.selectedCost);

                    Debug.Log($"Tile at ({gridX}, {gridY}) updated with material: {tileSelector.selectedMaterial.name}, cost: {tileSelector.selectedCost}");
                }
                break;
        }


    }

    private void OnMouseUp()
    {
        isHolding = false;
        isRandomized = false;
    }

    public void UpdateTile(Material newMaterial, float newCost)
    {
        // Update the tile's material and cost
        gridScript.UpdateTile(gridX, gridY, newMaterial, newCost);
        GetComponent<Renderer>().material = newMaterial;
        currentCost = newCost;
    }
    

    
}

