using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;

public class UIFeedbackInfo : MonoBehaviour
{
    public int StarValue;
    public InputField FeedbackInputField;


    public GameObject NextLevelObj;
    public GameObject RestartObj;
    public GameObject RateObj;

    List<GameObject> starGameObjects = new List<GameObject>();

    private GameManager gameManager;
    private UIManager uIManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        uIManager = FindObjectOfType<UIManager>();
        uIManager.WinObject = gameObject;
        StarValue = -1;
        createStarUILink("star01", 0);
        createStarUILink("star02", 1);
        createStarUILink("star03", 2);
        createStarUILink("star04", 3);
        createStarUILink("star05", 4);

        NextLevelObj.GetComponent<Button>().onClick.AddListener(gameManager.LoadNextLevel);
        RestartObj.GetComponent<Button>().onClick.AddListener(gameManager.RestartGame);
        RateObj.GetComponent<Button>().onClick.AddListener(RateLevel);
    }

    private void createStarUILink(string name, int index)
    {
        GameObject button = GameObject.Find(name);
        button.GetComponent<Image>().color = Color.black;
        button.GetComponent<Button>().onClick.AddListener(() => click(index));
        starGameObjects.Add(button);
    }



    void click(int index)
    {
        GameObject buttonObj = starGameObjects[index];
        Image image = buttonObj.GetComponent<Image>();
        if (image.color == Color.black)
        {
            StarValue = index + 1;
            for (int i = 0; i < starGameObjects.Count; i++)
            {
                if (index >= i)
                {
                    starGameObjects[i].GetComponent<Image>().color = Color.white;
                }
                else
                {
                    starGameObjects[i].GetComponent<Image>().color = Color.black;
                }
            }
        }
        else
        {
            StarValue = index;
            for (int i = 0; i < starGameObjects.Count; i++)
            {
                if (index <= i)
                {
                    starGameObjects[i].GetComponent<Image>().color = Color.black;
                }
            }
        }
    }

    void RateLevel()
    {
        Debug.Log("RateLevel");
        if (StarValue != -1)
        {
            Dictionary<string, object> feedback = new Dictionary<string, object>
        {
            {"Rating",  StarValue},
            {"FeedbackMessage",  FeedbackInputField.text},
            {"Playtime", gameManager.GetPlayTime() }
        };
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

            db.Collection("Feedback")
                .Document(SceneManager.GetActiveScene().name)
                .Collection("UserFeedback")
                .AddAsync(feedback)
                .ContinueWithOnMainThread(task =>
                {
                    DocumentReference addedDocRef = task.Result;
                    Debug.Log(System.String.Format("Added document with ID: {0}.", addedDocRef.Id));
                });
        }
    }
}
