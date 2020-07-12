using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject RestartButton;
    public GameObject WinObject;
    public GameObject RestartButtonEnd;
    public Text LevelName;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //RestartButton.GetComponent<Button>().onClick.AddListener(gameManager.RestartGame);
        //RestartButtonEnd.GetComponent<Button>().onClick.AddListener(gameManager.RestartGame);
        LevelName.text = gameManager.CurrentLevel;

        //UIFeedbackInfo = FindObjectOfType<UIFeedbackInfo>();
    }

    private void Update()
    {
        LevelName.text = gameManager.CurrentLevel;
    }
}
