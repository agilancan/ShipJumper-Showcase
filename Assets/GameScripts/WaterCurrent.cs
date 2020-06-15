using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour
{
    public Vector3 VelocityInfluence;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            NodeBehaviour nodeBehaviour = col.gameObject.GetComponent<NodeBehaviour>();
            if (!nodeBehaviour.VelocityInfluences.ContainsKey(gameObject.GetInstanceID()))
            {
                nodeBehaviour.VelocityInfluences.Add(gameObject.GetInstanceID(), VelocityInfluence);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            NodeBehaviour nodeBehaviour = col.gameObject.GetComponent<NodeBehaviour>();
            if (nodeBehaviour.VelocityInfluences.ContainsKey(gameObject.GetInstanceID()))
            {
                nodeBehaviour.VelocityInfluences.Remove(gameObject.GetInstanceID());
            }
        }
    }
}
