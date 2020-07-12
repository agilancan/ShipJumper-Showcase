using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public ChainManager ChainManager;
    public int ID;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //ChainManager.ChainCutter.ColliderInfo.text = "Out_" + ID + "_" + col.tag;
        if (col.tag == "chainCutter")
        {
            
            //ChainManager.ChainCutter.ColliderInfo.text = "In_" + ID;
            Chain chain = ChainManager.Chains.Find(c => c.ID == ID);
            if (chain.ChainType == ChainType.MindControl)
            {
                gameManager.Player.IsMindControlling = false;
            }
            if (chain == null)
            {
                chain = ChainManager.ConnectedChains.Find(c => c.ID == ID);
            }
            if (chain)
            {
                chain.SelfDestruct();
            }           
        }
    }
}
