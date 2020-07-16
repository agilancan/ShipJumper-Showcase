using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public GameManager gameManager;
    public SpriteRenderer SR;

    public bool GoalReached = false;
    public bool IsEndGoal = false;

    private void Awake()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.Goals.Add(this);
        if (IsEndGoal)
        {
            SR.color = Color.magenta;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            SR.color = Color.green;
            GoalReached = true;
            gameManager.CheckLevelCompletion();
        }
    }
}
