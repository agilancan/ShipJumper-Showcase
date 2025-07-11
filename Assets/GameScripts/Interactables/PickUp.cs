﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType
{
    Swapper,
    MindControl
}

public class PickUp : MonoBehaviour
{
    public PickUpType PickUpType;
    public float RespawnTime = 12;
    public GameManager gameManager;

    private float timeLeft = 0;
    public SpriteRenderer SR;

    //public bool GoalReached = false;

    private void Awake()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        //gameManager.Goals.Add(this);
    }

    private void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            SR.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(PickUpType == PickUpType.Swapper)
            {
                timeLeft = RespawnTime;
                SR.enabled = false;
                gameManager.ChainManager.CutAllChains();
                gameManager.Player.CurrentMode = Player.Mode.Swapper;
                gameManager.Player.SR.color = Color.black;
            }
            else
            {

            }
            //SR.color = Color.green;
            //GoalReached = true;
            //gameManager.CheckLevelCompletion();
        }
    }
}
