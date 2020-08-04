using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    public List<Power> PowerList = new List<Power>();
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void UpdatePlayerPowerStatus()
    {
        bool isPowered = false;

        foreach(Power power in PowerList)
        {
            if (power.IsConnected() && power.PowerType == PowerType.Source)
            {
                isPowered = true;
            }
        }
        gameManager.Player.PlayerStatus.IsPowered = isPowered;
    }
}
