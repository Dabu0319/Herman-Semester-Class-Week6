using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : SaveButton
{
    /// <summary>
    /// This script implements the Load button
    /// that checks for the existence of a file in the data folder,
    /// hides or shows the button based on the file's presence,
    /// and loads the grid based on the saved data
    /// </summary>
    
    // Reference to the Load button
    public GameObject loadButton;               // Reference to the Load button
    public string fileName = "GridData.txt";    // File itself

    private Dictionary<char, Material> charToMaterial;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        // Get self
        loadButton = gameObject;
        
        // Check if the file exists at game start
        if (!File.Exists(filePath))
        {
            // Hide the Load button if the file doesn't exist
            loadButton.gameObject.SetActive(false);
        }
        else
        {
            // Show the Load button if the file exists
            loadButton.gameObject.SetActive(true);
        }
        
        // Swap key and value
        charToMaterial = new Dictionary<char, Material>();
        foreach (var kvp in materialToChar)
        {
            charToMaterial[kvp.Value] = kvp.Key;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGrid()
    {
        // Get the grid array from GridScript
        GameObject[,] gridArray = gridScript.GetGrid();
        
        if (File.Exists(filePath))
        {
            string[] fileLines = File.ReadAllLines(filePath); // Read each line of the file

            for (int y = 0; y < fileLines.Length; y++)
            {
                for (int x = 0; x < fileLines[y].Length; x++)
                {
                    char tileChar = fileLines[y][x];
                    
                    // Retrieve the corresponding material from the dictionary
                    if (charToMaterial.TryGetValue(tileChar, out Material newMaterial))
                    {
                        // Update the tile's material and cost
                        gridScript.UpdateTile(x, y, newMaterial, gridScript.GetMovementCost(gridArray[x, y]));
                    }
                    else
                    {
                        Debug.LogError($"Unrecognized character '{tileChar}' at ({x}, {y})");
                    }
                }
            }
            
            Debug.Log("Grid loaded successfully.");
        }
        else
        {
            Debug.LogError("No saved grid file found.");
        }
    }
}
