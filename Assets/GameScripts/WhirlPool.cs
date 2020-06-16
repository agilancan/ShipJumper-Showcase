using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlPool : MonoBehaviour
{
    public float PullForce;
    public Transform TargetDestination;

    private Vector2 currentShipDirection;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player player = col.gameObject.GetComponent<Player>();
        }
    }
}
