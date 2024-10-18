using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTiles : MonoBehaviour
{
    public GridScript gridScript;
    
    void Start()
    {
         gridScript = FindObjectOfType<GridScript>();    
    }

    // Update is called once per frame
   
    public void RandomizeTiles()
    {
        for (int x = 0; x < gridScript.gridWidth; x++)
        {
            for (int y = 0; y < gridScript.gridHeight; y++)
            {
                int randomIndex = UnityEngine.Random.Range(0, gridScript.mats.Length);
                Material randomMaterial = gridScript.mats[randomIndex];
                float randomCost = gridScript.costs[randomIndex];
                // Update the tile's material and cost
                gridScript.UpdateTile( x, y, randomMaterial, randomCost);
            }
        }
    }
}
