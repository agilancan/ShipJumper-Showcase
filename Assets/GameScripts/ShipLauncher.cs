using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLauncher : MonoBehaviour
{
    public float LaunchForce = 2;
    public float LaunchTime = 1;
    public float SpeedLossFactor = 0.01f;

    public float CoolDown = 3;

    private float lauchTimeLeft = 0;
    private Vector2 direction;
    private float currentForce;
    private Player player;
    private NodeBehaviour playerNodeBehaviour;

    public void LaunchPLayer(Vector3 targetPosition)
    {
        if (player)
        {
            direction = (targetPosition - player.transform.position).normalized;
            playerNodeBehaviour.VelocityOverride = direction * LaunchForce;
            player.CurrentMode = Player.Mode.Launching;
            lauchTimeLeft = LaunchTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player p = col.gameObject.GetComponent<Player>();
            if(p.GetSL() != this)
            {
                p.GameManager.ChainManager.CutAllChains();
                if (p.GetSL())
                {                    
                    p.GetSL().ResetLauncher();
                }                
                player = p;
                playerNodeBehaviour = col.gameObject.GetComponent<NodeBehaviour>();
                player.ToggleLaunchMode(true, this);
            }         
        }
    }

    public void ResetLauncher()
    {
        player.ToggleLaunchMode(false, this);
        currentForce = LaunchForce;
        lauchTimeLeft = LaunchTime;
        player = null;
    }
    private void FixedUpdate()
    {
        if (player)
        {
            if(player.CurrentMode == Player.Mode.Launching && !player.GameManager.IsGamePaused)
            {
                if (lauchTimeLeft >= 0)
                {
                    lauchTimeLeft -= Time.deltaTime;
                    currentForce -= SpeedLossFactor;
                    if(currentForce >= 0)
                    {
                        playerNodeBehaviour.VelocityOverride = direction * currentForce;
                    }                    
                }
                else
                {
                    ResetLauncher();
                }
            }            
        }
    }
}
