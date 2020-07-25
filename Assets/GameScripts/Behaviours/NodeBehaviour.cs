using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour : MonoBehaviour
{
    public ConstantForce2D ConstantForce;
    public Rigidbody2D NodeRB;
    public Vector3 Velocity;

    public Dictionary<int, Vector3> VelocityInfluences = new Dictionary<int, Vector3>();

    public Vector3 VelocityOverride;
    public bool IsOverrideEnabled = false;

    public Power Power;

    private GameManager gameManager;
    private Player player;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = gameManager.Player;
        Power = GetComponent<Power>();
        ConstantForce = GetComponent<ConstantForce2D>();
        NodeRB = GetComponent<Rigidbody2D>();
        //NodeRB.velocity = Velocity;
        ConstantForce.force = Velocity;
        IsOverrideEnabled = false;
    }


    private void moveNodeUpdate()
    {
        if (IsOverrideEnabled)
        {
            NodeRB.velocity = VelocityOverride;
            //NodeRB.ad
        }
        else
        {
            Vector3 velocity = Velocity;
            foreach (Vector3 v in VelocityInfluences.Values)
            {
                velocity += v;
            }
            NodeRB.velocity = velocity;
        }
    }
    private void FixedUpdate()
    {
        if (Power)
        {
            if(Power.PowerType == PowerType.Drain)
            {
                if(player.PlayerStatus.IsPowered
                && Power.IsConnected())
                {
                    moveNodeUpdate();
                }
                else
                {
                    NodeRB.velocity = Vector2.zero;
                }
            }
            else
            {
                moveNodeUpdate();
            }
        }
        else
        {
            moveNodeUpdate();
        }                  
    }
}
