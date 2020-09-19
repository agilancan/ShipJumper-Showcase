using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAdjuster : MonoBehaviour
{
    private GameManager gameManager;
    private SpriteRenderer SR;
    private Player player;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = gameManager.Player;
        NodeBehaviour nb = player.gameObject.GetComponent<NodeBehaviour>();
        if(nb.Velocity.x < 0)
        {
            transform.Rotate(new Vector3(0, 0, -180));
        }
        else if (nb.Velocity.y > 0)
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }
        else if (nb.Velocity.y < 0)
        {
            transform.Rotate(new Vector3(0, 0, -90));
        }

    }
}
