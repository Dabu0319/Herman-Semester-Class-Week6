using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Draw : MonoBehaviour
{
    public bool canDraw = true;
    void Update()
    {
        if (Input.GetMouseButton(0) && canDraw)
        {
            // Raycast from the mouse position to detect GameObjects
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object hit by the ray is a TileComponent
                TileComponent tile = hit.transform.GetComponent<TileComponent>();

                if (tile != null)
                {
                    tile.OnMouseDown();
                }
            }
        }
        
        
       
    }
}
