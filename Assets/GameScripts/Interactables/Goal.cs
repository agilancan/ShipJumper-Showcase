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

    private Color startingColor;

    private void Awake()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.Goals.Add(this);
        SR.sortingLayerName = "Player";
        startingColor = SR.color;
        SR.color = new Color(startingColor.r, startingColor.g, startingColor.b, .8f);
        if (IsEndGoal)
        {
            SR.color = new Color(1, 0, 1, .8f);
            //SR.color = Color.magenta;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            SR.color = new Color(0, 1, 0, .8f);
            //SR.color = Color.green;
            GoalReached = true;
            gameManager.CheckLevelCompletion();
        }
    }
}
