using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public GameManager GameManager;
    public SpriteRenderer SR;

    public bool GoalReached = false;

    private void Start()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            SR.color = Color.green;
            GoalReached = true;
            GameManager.CheckLevelCompletion();
        }
    }
}
