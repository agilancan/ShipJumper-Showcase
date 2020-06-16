using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager UIManager;
    public InputController InputController;
    public ChainManager ChainManager;

    public List<Goal> Goals = new List<Goal>();

    public string NextLevel = "";

    private void Start()
    {
        ResumeGame();
    }

    public void CheckLevelCompletion()
    {
        bool levelComplete = true;
        foreach(Goal goal in Goals)
        {
            if (!goal.GoalReached)
            {
                levelComplete = false;
            }
        }
        if (levelComplete)
        {
            UIManager.WinObject.gameObject.SetActive(true);
            PauseGame();
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(NextLevel);
    }

    public void RestartGame() 
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
