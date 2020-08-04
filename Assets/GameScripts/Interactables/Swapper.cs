using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swapper : MonoBehaviour
{
    private GameManager gameManager;
    private Player player;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = gameManager.Player;
    }
    public void ActivateSwapperMode()
    {
        player.CurrentMode = Player.Mode.Swapper;
        player.SR.color = Color.black;
    }
}
