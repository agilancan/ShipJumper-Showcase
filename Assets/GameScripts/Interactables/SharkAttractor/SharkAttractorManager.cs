using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttractorManager : MonoBehaviour
{
    public List<SharkAttractor> SharkAttractorList = new List<SharkAttractor>();
    public SharkAttractor ActiveSharkAttractor;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

}
