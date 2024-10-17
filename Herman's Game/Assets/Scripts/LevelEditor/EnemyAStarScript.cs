using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class EnemyAStarScript : MonoBehaviour
{
    public bool visualizeGridSpacesVisited = true;

    public GridScript gridScript;
    public HeuristicScript heuristic;
    public Transform princess;  // Reference to the princess

    private int gridWidth;
    private int gridHeight;

    private GameObject[,] pos;

    // A* Variables
    private Vector3 start;
    private Vector3 goal;
    public Path path;

    private const int MAX_LOCATIONS_IN_QUEUE = 1000;
    private FastPriorityQueue<PriorityQueueVector3> frontier;

    private Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
    private Dictionary<Vector3, float> costSoFar = new Dictionary<Vector3, float>();
    private Vector3 current;

    public bool isPathfindingComplete = false;
    private bool isPathfindingActive = false;

    // StartGame will trigger A* search when called
    public void StartGame()
    {
        if (!isPathfindingActive)
        {
            isPathfindingActive = true;
            StartCoroutine(InitAstarCoroutine());
        }
    }

    private IEnumerator InitAstarCoroutine()
    {
        yield return StartCoroutine(InitAstarCoroutine(new Path(heuristic.gameObject.name, gridScript)));
    }

private IEnumerator InitAstarCoroutine(Path newPath)
{
    path = newPath;
    pos = gridScript.GetGrid();
    gridWidth = gridScript.gridWidth;
    gridHeight = gridScript.gridHeight;

    // Convert enemy and princess positions to grid coordinates
    Vector3Int startGridPos = WorldToGrid(transform.position);  // Enemy's start position
    Vector3Int goalGridPos = WorldToGrid(princess.position);    // Princess's goal position

    // Ensure start and goal are within grid bounds
    if (startGridPos.x < 0 || startGridPos.x >= gridWidth || startGridPos.y < 0 || startGridPos.y >= gridHeight)
    {
        Debug.LogError("Start position is out of grid bounds: " + startGridPos);
        yield break;
    }

    if (goalGridPos.x < 0 || goalGridPos.x >= gridWidth || goalGridPos.y < 0 || goalGridPos.y >= gridHeight)
    {
        Debug.LogError("Goal position is out of grid bounds: " + goalGridPos);
        yield break;
    }

    // Initialize the frontier with the correct grid start position
    frontier = new FastPriorityQueue<PriorityQueueVector3>(MAX_LOCATIONS_IN_QUEUE);
    frontier.Enqueue(new PriorityQueueVector3(startGridPos), 0);

    cameFrom.Clear();
    costSoFar.Clear();

    cameFrom.Add(startGridPos, startGridPos);
    costSoFar.Add(startGridPos, 0);

    int exploredNodes = 0;

    // A* Algorithm Loop
    while (frontier.Count != 0)
    {
        exploredNodes++;
        current = frontier.Dequeue().Vector;

        if (current.Equals(goalGridPos))
        {
            Debug.Log("Goal Reached at: " + current);
            break;
        }

        for (int x = -1; x < 2; x += 2)
        {
            AddNodesToFrontier((int)current.x + x, (int)current.y);
        }

        for (int y = -1; y < 2; y += 2)
        {
            AddNodesToFrontier((int)current.x, (int)current.y + y);
        }
    }

    if (!current.Equals(goalGridPos))
    {
        Debug.LogError("A* failed to reach the goal.");
        yield break;
    }

    path.nodeInspected = exploredNodes;

    // Reconstruct the path
    ReconstructPath();

    isPathfindingComplete = true;
    isPathfindingActive = false;

    yield return null;
}


    private void AddNodesToFrontier(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            Vector3 next = new Vector3(x, y);
            float newCost = costSoFar[current] + gridScript.GetMovementCost(pos[x, y]);

            // Debugging: Log cost and neighbor being added
            Debug.Log("Adding neighbor: " + next + " with cost: " + newCost);

            if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
            {
                costSoFar[next] = newCost;
                float priority = newCost + heuristic.Heuristic(x, y, start, goal, gridScript);
                frontier.Enqueue(new PriorityQueueVector3(next), priority);
                cameFrom[next] = current;
            }
        }
        else
        {
            Debug.LogWarning("Tried to add node out of bounds: " + new Vector3(x, y));
        }
    }

    private void ReconstructPath()
    {
        current = goal;

        path.path.Clear();

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.LogError("Goal position not found in cameFrom. Path reconstruction failed.");
            return;
        }

        while (!current.Equals(start))
        {
            if (!cameFrom.ContainsKey(current))
            {
                Debug.LogError("Current position not found in cameFrom: " + current);
                return;
            }

            GameObject go = pos[(int)current.x, (int)current.y];

            if (go != null)
            {
                path.Insert(0, go, new Vector3((int)current.x, (int)current.y));
            }

            current = cameFrom[current];
        }

        if (pos[(int)start.x, (int)start.y] != null)
        {
            path.Insert(0, pos[(int)start.x, (int)start.y]);
        }
    }
    
    public Vector3Int WorldToGrid(Vector3 worldPos)
    {
        // Assuming gridScript provides a method for grid spacing and origin
        float gridSpacing = gridScript.spacing;
        Vector3 gridOrigin = gridScript.transform.position;  // Origin of the grid in world space

        int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / gridSpacing);
        int y = Mathf.FloorToInt((worldPos.y - gridOrigin.y) / gridSpacing);

        return new Vector3Int(x, y, 0);  // Return as grid position
    }
}
