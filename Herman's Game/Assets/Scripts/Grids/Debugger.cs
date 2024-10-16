using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    public TMP_Text text;
    public GridScript _gridScript;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int gridX = Mathf.FloorToInt((mousePos.x + (_gridScript.gridWidth * _gridScript.spacing) / 2f) / _gridScript.spacing);
        int gridY = Mathf.FloorToInt(((_gridScript.gridHeight * _gridScript.spacing) / 2f - mousePos.y) / _gridScript.spacing);
        
        text.text = "Current Grid: " + new Vector2(gridX, gridY);
    }
}
