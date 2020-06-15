using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endlink : MonoBehaviour
{
    public GameObject FirstLink;
    public Chain Chain;
    public int type;
    static public HingeJoint2D HingeJoint;
    static public HingeJoint2D HingeJoint2;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "anchorObj")
        {
            if(type == 0)
            {
                HingeJoint = col.gameObject.GetComponent<HingeJoint2D>();
                HingeJoint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                Chain.ExecuteEndLine();
            }
            else
            {
                HingeJoint2 = col.gameObject.GetComponent<HingeJoint2D>();
                HingeJoint2.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                Chain.ExecuteEndLine();
            }
            
        }
    }
}
