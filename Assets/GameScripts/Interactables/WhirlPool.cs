using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlPool : MonoBehaviour
{
    public Power Power;
    public float PullForce;
    public Transform TargetDestination;

    private NodeBehaviour playerNodeBehaviour;
    private GameManager gameManager;

    int id;

    private Vector2 targetDirection = Vector2.zero;
    private void Start()
    {
        id = gameObject.GetInstanceID();
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerNodeBehaviour = col.gameObject.GetComponent<NodeBehaviour>();
            if (!playerNodeBehaviour.VelocityInfluences.ContainsKey(id))
            {
                Vector2 direction = Vector2.zero;
                if (Power)
                {
                    if ((Power.IsConnected() && gameManager.Player.PlayerStatus.IsPowered) 
                        || Power.PowerType == PowerType.Source)
                    {
                        direction =
                        (TargetDestination.position - playerNodeBehaviour.gameObject.transform.position).normalized;
                    }
                }
                else
                {
                    direction =
                    (TargetDestination.position - playerNodeBehaviour.gameObject.transform.position).normalized;
                }
                playerNodeBehaviour.VelocityInfluences.Add(id, direction * PullForce);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!playerNodeBehaviour)
            {
                playerNodeBehaviour = col.gameObject.GetComponent<NodeBehaviour>();
            }
            
            if (playerNodeBehaviour.VelocityInfluences.ContainsKey(id))
            {
                playerNodeBehaviour.VelocityInfluences.Remove(id);
                playerNodeBehaviour = null;
            }
        }
    }

    private void Update()
    {
        if (playerNodeBehaviour)
        {            
            if (playerNodeBehaviour.VelocityInfluences.ContainsKey(id))
            {
                if (Power)
                {
                    if ((Power.IsConnected() && gameManager.Player.PlayerStatus.IsPowered)
                        || Power.PowerType == PowerType.Source)
                    {
                        targetDirection = (TargetDestination.position - playerNodeBehaviour.gameObject.transform.position).normalized;
                    }
                    else
                    {
                        targetDirection = Vector2.zero;
                    }
                }
                else
                {
                    targetDirection = (TargetDestination.position - playerNodeBehaviour.gameObject.transform.position).normalized;
                }
                playerNodeBehaviour.VelocityInfluences[id] = targetDirection * PullForce;
            }
        }
    }
}
