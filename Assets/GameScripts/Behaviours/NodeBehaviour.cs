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
    
    void Start()
    {
        ConstantForce = GetComponent<ConstantForce2D>();
        NodeRB = GetComponent<Rigidbody2D>();
        //NodeRB.velocity = Velocity;
        ConstantForce.force = Velocity;
        IsOverrideEnabled = false;
    }

    void FixedUpdate()
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
}
