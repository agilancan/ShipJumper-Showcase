using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public ChainManager ChainManager;
    public int ID;

    private void OnTriggerEnter2D(Collider2D col)
    {
        //ChainManager.ChainCutter.ColliderInfo.text = "Out_" + ID + "_" + col.tag;
        if (col.tag == "chainCutter")
        {
            //ChainManager.ChainCutter.ColliderInfo.text = "In_" + ID;
            Chain chain = ChainManager.Chains.Find(c => c.ID == ID);
            if (chain)
            {
                chain.SelfDestruct();
            }            
        }
    }
}
