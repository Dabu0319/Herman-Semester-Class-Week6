using UnityEngine;

public class EnemyFollowAStarScript : MonoBehaviour
{
    private bool move = false;
    private Path path;
    public EnemyAStarScript astar;
    private Step startPos;
    private Step destPos;
    private int currentStep = 0;

    private float lerpPer = 0;
    private float travelStartTime;

    private bool isInitialized = false;

    // Update is called once per frame
    private void Update()
    {
        if (astar.isPathfindingComplete && !move && !isInitialized)
        {
            InitializePathFollowing();
        }

        if (move)
        {
            // Move between startPos and destPos using Lerp
            lerpPer += Time.deltaTime / destPos.moveCost;
            transform.position = Vector3.Lerp(startPos.gameObject.transform.position, destPos.gameObject.transform.position, lerpPer);

            // Check if movement has completed for this step
            if (lerpPer >= 1)
            {
                lerpPer = 0; // Reset lerp percentage
                currentStep++; // Move to the next step

                // Check if currentStep is within the bounds of the path steps
                if (currentStep >= path.path.Count)
                {
                    // Reached the goal
                    move = false;
                    Debug.Log(path.pathName + " reached the goal.");
                    currentStep = 0;  // Reset for next pathfinding
                }
                else
                {
                    // Continue to the next step
                    startPos = destPos;
                    destPos = path.Get(currentStep);
                }
            }
        }
    }

    private void StartMove()
    {
        move = true;
        travelStartTime = Time.realtimeSinceStartup;
    }

    private void InitializePathFollowing()
    {
        path = astar.path;

        // Ensure the path has steps before proceeding
        if (path.path.Count == 0)
        {
            Debug.LogError("Path is empty, cannot follow!");
            return;
        }

        // Start at the first position
        startPos = path.Get(0);
        destPos = path.Get(currentStep);

        // Set the enemy's position to the start of the path
        transform.position = startPos.gameObject.transform.position;

        // Start moving after a small delay
        Invoke("StartMove", path.nodeInspected / 100f);

        isInitialized = true;
    }

    public void FollowPath()
    {
        if (!move && !isInitialized)
        {
            InitializePathFollowing();
        }
    }
}
