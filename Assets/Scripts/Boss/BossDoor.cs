 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoor : MonoBehaviour
{
    private enum Direction
    { In, Out}
    [SerializeField] private Direction direction;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        switch (direction)
        {
            case Direction.In:
                var async1 = SceneManager.LoadSceneAsync("BossScene", LoadSceneMode.Additive);
                async1.completed += SwitchLevel;
                break;

            case Direction.Out:
                var async2 = SceneManager.LoadSceneAsync("NewLevel", LoadSceneMode.Additive);
                async2.completed += SwitchLevel;
                TestDifficultyScaler.currentLevel++;
                break;
        }
    }

    private void SwitchLevel(AsyncOperation operation)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
    }
}
