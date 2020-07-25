using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlPool : MonoBehaviour
{
    public float PullForce;
    public Transform TargetDestination;

    private NodeBehaviour playerNodeBehaviour;

    int id;
    private void Start()
    {
        id = gameObject.GetInstanceID();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerNodeBehaviour = col.gameObject.GetComponent<NodeBehaviour>();
            if (!playerNodeBehaviour.VelocityInfluences.ContainsKey(id))
            {
                Vector2 direction = 
                    (TargetDestination.position - playerNodeBehaviour.gameObject.transform.position).normalized;
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
                Vector2 direction = 
                    (TargetDestination.position - playerNodeBehaviour.gameObject.transform.position).normalized;
                playerNodeBehaviour.VelocityInfluences[id] = direction * PullForce;
                Debug.Log(playerNodeBehaviour.VelocityInfluences[id]);
            }
        }
    }
}
