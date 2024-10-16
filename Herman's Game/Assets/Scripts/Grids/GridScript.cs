using UnityEngine;
using System.Collections;

public class GridScript : MonoBehaviour {

	public int gridWidth;
    public int gridHeight;
    public float spacing;

    public Material[] mats;
    public float[] costs;

    public Vector3 start = new Vector3(0, 0);
    public Vector3 goal = new Vector3(0, 0);

    GameObject[,] gridArray;

    public GameObject startSprite;
    public GameObject goalSprite;

    protected virtual void Start()
    {
        // I changed it to generate the grid when the game starts
        // so that it can be edited
        GetGrid();
    }
    
	public virtual GameObject[,] GetGrid()
	{
		if (gridArray == null)
		{
			Debug.Log(start);
			Debug.Log(goal);

			gridArray = new GameObject[gridWidth, gridHeight];

			float offsetX = (gridWidth * -spacing) / 2f;
			float offsetY = (gridHeight * spacing) / 2f;

			for (int x = 0; x < gridWidth; x++)
			{
				for (int y = 0; y < gridHeight; y++)
				{
					GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
					quad.transform.localScale = new Vector3(spacing, spacing, spacing);
					quad.transform.position = new Vector3(offsetX + x * spacing,
					offsetY - y * spacing, 0);

					quad.transform.parent = transform;

					gridArray[x, y] = quad;

					// Set the material for each tile
					quad.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial(x, y);

					// Add a TileComponent to each tile
					TileComponent tileComponent = quad.AddComponent<TileComponent>();
					tileComponent.Initialize(this, x, y, quad.GetComponent<MeshRenderer>().sharedMaterial, GetMovementCost(quad));

					// Start and destination points
					if (Mathf.Approximately(goal.x, x) && Mathf.Approximately(goal.y, y))
					{
						goalSprite.transform.position = quad.transform.position;
						goalSprite.AddComponent<Draggable>(); // Add the "draggable" component
					}
					if (Mathf.Approximately(start.x, x) && Mathf.Approximately(start.y, y))
					{
						startSprite.transform.position = quad.transform.position;
						startSprite.AddComponent<Draggable>(); // Add the "draggable" component
					}
				}
			}
		}

		return gridArray;
	}

	// Updates the tile's texture and movement cost based on the player's choice
    public void UpdateTile(int gridX, int gridY, Material newMaterial, float newCost)
    {
        GameObject tile = gridArray[gridX, gridY];
        tile.GetComponent<Renderer>().material = newMaterial;

        Debug.Log($"Updated tile at ({gridX}, {gridY}) with new material {newMaterial.name} and cost {newCost}");
    }

    public virtual float GetMovementCost(GameObject go)
    {
        Material mat = go.GetComponent<MeshRenderer>().sharedMaterial;
        int i;

        for (i = 0; i < mats.Length; i++)
        {
            if (mat.name.StartsWith(mats[i].name))
            {
                break;
            }
        }

        return costs[i];
    }
    protected virtual Material GetMaterial(int x, int y)
    {
        return mats[0]; // return mats0 is the default grass material
    }

	void OnDrawGizmos() // 就看了一下mat这个Grid是怎么搞的 原来对齐的是grid的交叉点
    {
        if (Application.isPlaying)  
        {
            DrawGrid();
        }
    }

    private void DrawGrid()
    {
        float offsetX = (gridWidth * -spacing) / 2f;
        float offsetY = (gridHeight * spacing) / 2f;

        Gizmos.color = Color.green;

        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = new Vector3(offsetX, offsetY - y * spacing, 0);
            Vector3 end = new Vector3(offsetX + gridWidth * spacing, offsetY - y * spacing, 0);
            Gizmos.DrawLine(start, end);
        }

        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = new Vector3(offsetX + x * spacing, offsetY, 0);
            Vector3 end = new Vector3(offsetX + x * spacing, offsetY - gridHeight * spacing, 0);
            Gizmos.DrawLine(start, end);
        }
    }

}
