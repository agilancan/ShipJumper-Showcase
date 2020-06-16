using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLauncher : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player player = col.gameObject.GetComponent<Player>();
        }
    }
}
