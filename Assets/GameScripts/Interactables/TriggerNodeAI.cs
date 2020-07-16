using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNodeAI : MonoBehaviour
{
    private NodeBehaviour nodeBehaviour;
    private HingeJoint2D hingeJoint2D;
    private Rigidbody2D rb;
    private Vector3 originalPosition;

    public Vector2 TriggerVelocity;
    public bool IsMoving = false;

    void Start()
    {
        nodeBehaviour = GetComponent<NodeBehaviour>();
        hingeJoint2D = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        originalPosition = gameObject.transform.position;
        IsMoving = false;
    }

    void Update()
    {
        if(hingeJoint2D.connectedBody == null || hingeJoint2D.connectedBody == rb)
        {
            gameObject.transform.position = originalPosition;
            nodeBehaviour.Velocity = Vector3.zero;
            IsMoving = false;
        }
    }

    public void ExecuteTrigger()
    {
        nodeBehaviour.Velocity = TriggerVelocity;
        IsMoving = true;
    }
}
