using System.Collections;
using Firebase;
using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;

public class GameManager : MonoBehaviour
{
    public Player Player;
    public UIManager UIManager;
    public InputController InputController;
    public ChainManager ChainManager;
    public PowerManager PowerManager;

    public List<Goal> Goals;

    public string NextLevel = "";
    public string CurrentLevel = "";

    public bool IsGamePaused = false;

    private float playTime = 0;

    private void Start()
    {
        /*FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(continuationAction: task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });*/
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ResumeGame();
    }

    private void Update()
    {
        if (!IsGamePaused)
        {
            playTime += Time.deltaTime;
        }         
    }

    public float GetPlayTime()
    {
        return playTime;
    }

    public void CheckLevelCompletion()
    {
        bool levelComplete = true;
        bool endGoalReached = false;
        foreach(Goal goal in Goals)
        {
            if (goal.IsEndGoal && goal.GoalReached)
            {
                endGoalReached = true;
            }
            if (!goal.GoalReached)
            {
                levelComplete = false;
            }
        }
        if (levelComplete || endGoalReached)
        {
            UIManager.WinObject.gameObject.SetActive(true);
            uploadPlayTime();
            PauseGame();
        }
    }

    private void uploadPlayTime()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Dictionary<string, object> playTime = new Dictionary<string, object>
        {
            {"LevelName",  CurrentLevel},            
            {"playtime", GetPlayTime() },
            {"timestamp", FieldValue.ServerTimestamp }
    };
        db.Collection("LevelPlayTimes")
                .AddAsync(playTime);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(NextLevel);
    }

    public void Unstable_LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void RestartGame() 
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1;
    }

}
