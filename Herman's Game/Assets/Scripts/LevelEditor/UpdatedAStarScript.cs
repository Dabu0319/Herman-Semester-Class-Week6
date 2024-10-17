using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdatedAStarScript : AStarScript
{
    public void StartGame()
    {
        if (!isPathfindingActive && !isPathfindingComplete)
        {
            isPathfindingActive = true;
            StartCoroutine(InitAstarCoroutine());
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Scenes/LevelEditor");
    }
    
    
}
