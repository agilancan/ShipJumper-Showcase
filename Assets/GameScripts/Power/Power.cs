using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerType
{
    Source,
    Drain
}

public class Power : MonoBehaviour
{
    public PowerType PowerType;

    [SerializeField]
    private bool isConnected = false;

    private GameManager gameManager;
    private PowerManager powerManager;

    public bool IsConnected()
    {
        return isConnected;
    }

    public void Connect()
    {
        isConnected = true;
        powerManager.UpdatePlayerPowerStatus();
    }

    public void Disconnect()
    {
        isConnected = false;
        powerManager.UpdatePlayerPowerStatus();
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        powerManager = gameManager.PowerManager;
        powerManager.PowerList.Add(this);
    }

    private void OnDestroy()
    {
        powerManager.PowerList.Remove(this);
        powerManager.UpdatePlayerPowerStatus();
    }
}
