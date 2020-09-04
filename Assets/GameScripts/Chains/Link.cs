using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public ChainManager ChainManager;
    public int ID;
    public Chain Chain;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "chainCutter")
        {
            Chain.SelfDestruct();
        }
    }
}
