using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveButton : MonoBehaviour
{
    /// <summary>
    /// This script is for players to click the "Save" button
    /// to save the customized map after finishing editing the map.
    /// </summary>

    // Reference to the GridScript
    public GridScript gridScript;

    // Path to save the file
    public string filePath;
    
    // Material-to-char mapping
    public Dictionary<Material, char> materialToChar;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Setup
        gridScript = GameObject.Find("Grid").GetComponent<GridScript>();
        filePath = Application.dataPath + "/Data/GridData.txt";
        
        // Materials
        materialToChar = new Dictionary<Material, char>()
        {
            { gridScript.mats[0], '-'}, // Grass
            { gridScript.mats[1], 'l'}, // Lava
            { gridScript.mats[2], 'f'}, // Forest
            { gridScript.mats[3], 'p'}, // Path
            { gridScript.mats[4], 'r'}, // Rock
            { gridScript.mats[5], 'w'}, // Water
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // When the Save button is called
    public void SaveGrid()
    {
        string gridData = "";

        // Get the grid array from GridScript
        GameObject[,] gridArray = gridScript.GetGrid();

        for (int y = 0; y < gridScript.gridHeight; y++)
        {
            for (int x = 0; x < gridScript.gridWidth; x++)
            {
                GameObject tile = gridArray[x, y];
                Material tileMaterial = tile.GetComponent<MeshRenderer>().sharedMaterial; // Get the tile's material
                // NOTE: "tile.GetComponent<MeshRenderer>().material" won't work, why ???
                
                // Get the corresponding char for the material
                if (materialToChar.TryGetValue(tileMaterial, out char materialChar))
                {
                    gridData += materialChar;
                }
                else
                {
                    gridData += '?'; // Default to '?' if material is not found in the map
                    Debug.Log("NO TILE MAT FOUND.");
                }
            }
            
            gridData += "\n"; // New line for each row of tile
        }
        
        // Write the grid data to a file
        File.WriteAllText(filePath, gridData);
        Debug.Log($"Grid data saved to {filePath}.");
    }
}
