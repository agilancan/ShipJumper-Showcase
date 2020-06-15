using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    public GameManager GameManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameManager.UIManager.RestartButton.gameObject.SetActive(true);
            GameManager.PauseGame();
        }
    }
}
