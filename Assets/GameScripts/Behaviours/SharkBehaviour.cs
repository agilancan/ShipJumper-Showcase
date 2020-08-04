using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBehaviour : MonoBehaviour
{    
    public Rigidbody2D NodeRB;
    public bool PowerAttracted;
    public float MovementSpeed;

    [SerializeField]
    private Vector2 currentVelocity;

    private GameManager gameManager;
    private Player player;
    private SharkAttractorManager sharkAttractorManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        sharkAttractorManager = gameManager.SharkAttractorManager;
        player = gameManager.Player;
        NodeRB = GetComponent<Rigidbody2D>();
    }

    private Vector2 getSharkVelocity(Vector2 targetPosition)
    {
        Vector2 sharkPosition = gameObject.transform.position;
        Vector2 velocity = (targetPosition - sharkPosition).normalized * MovementSpeed;
        return velocity;
    }

    private void FixedUpdate()
    {
        //Goes after active attractor as priority
        if (sharkAttractorManager.ActiveSharkAttractor)
        {
            NodeRB.velocity = getSharkVelocity(
                sharkAttractorManager.ActiveSharkAttractor.gameObject.transform.position);
        }
        //Goes after player if player is powered
        else if (PowerAttracted)
        {
            if (player.PlayerStatus.IsPowered)
            {
                NodeRB.velocity = getSharkVelocity(player.gameObject.transform.position);
            }
            else
            {
                NodeRB.velocity = Vector2.zero;
            }
        }
        //Goes after player if player is unpowered
        else
        {
            if (!player.PlayerStatus.IsPowered)
            {
                NodeRB.velocity = getSharkVelocity(player.gameObject.transform.position);
            }
            else
            {
                NodeRB.velocity = Vector2.zero;
            }
        }
    }
}
