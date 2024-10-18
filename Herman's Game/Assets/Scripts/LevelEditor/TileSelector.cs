using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public Material selectedMaterial;  
    public float selectedCost;
    public List<GameObject> previewPrefabs; 
    private GameObject currentPreview;  

    public TileType tileType = TileType.Basic;
    
    public void SelectTile(Material newMaterial, float newCost)
    {
        selectedMaterial = newMaterial;
        selectedCost = newCost;
        Debug.Log("Selected material: " + newMaterial.name + " with cost: " + newCost);
    }

    void Update()
    {
       
        if (currentPreview != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -1;  
            currentPreview.transform.position = mousePos;  
        }
        
        
        if (Input.GetMouseButton(1))
        {
            selectedMaterial = null;
            selectedCost = 0;
            StopTilePreview();
        }
    }
    
    public void StartTilePreview(string previewName)
    {
        
        StopTilePreview();

        
        foreach (GameObject prefab in previewPrefabs)
        {
            if (prefab.name == previewName)
            {
                
                currentPreview = Instantiate(prefab);
                currentPreview.SetActive(true);  
                Debug.Log("Preview started: " + previewName);
                return;
            }
        }

        Debug.LogWarning("No preview prefab found for: " + previewName);
    }

    
    public void StopTilePreview()
    {
        
        if (currentPreview != null)
        {
            Destroy(currentPreview);  
            currentPreview = null;
        }
    }
}