using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameManager.UIManager.RestartButton.gameObject.SetActive(true);
            gameManager.PauseGame();
        }
    }
}
