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
        if (isHolding)
        {
            DrawTile();
        }
    }

    void OnMouseDown()
    {
        isHolding = true;
    }

    private void DrawTile()
    {
        // Check if there is a selected material and cost,
        // if not, do not perform the replacement operation
        if (tileSelector != null && tileSelector.selectedMaterial != null && tileSelector.selectedCost > 0)
        {
            // Update the tile's material and cost
            gridScript.UpdateTile(gridX, gridY, tileSelector.selectedMaterial, tileSelector.selectedCost);

            GetComponent<Renderer>().material = tileSelector.selectedMaterial;
            currentCost = tileSelector.selectedCost;

            Debug.Log($"Tile at ({gridX}, {gridY}) updated with material: {tileSelector.selectedMaterial.name}, cost: {tileSelector.selectedCost}");
        }
    }

    private void OnMouseUp()
    {
        isHolding = false;
    }
}

