using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttractor : MonoBehaviour
{
    public Chain ConnectedChain;

    private GameManager gameManager;
    private SharkAttractorManager sharkAttractorManager;
    private bool isActive;

    public bool IsActive()
    {
        return isActive;
    }

    public void Activate(Chain chain)
    {
        ConnectedChain = chain;
        isActive = true;
        if (sharkAttractorManager.ActiveSharkAttractor)
        {
            sharkAttractorManager.ActiveSharkAttractor.Deactivate();
        }        
        sharkAttractorManager.ActiveSharkAttractor = this;
    }

    public void Deactivate()
    {
        sharkAttractorManager.ActiveSharkAttractor = null;
        ConnectedChain = null;
        isActive = false;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        sharkAttractorManager = gameManager.SharkAttractorManager;
        sharkAttractorManager.SharkAttractorList.Add(this);
    }
}
