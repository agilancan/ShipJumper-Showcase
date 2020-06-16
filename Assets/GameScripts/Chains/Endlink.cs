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

    void ExecuteEndLine(Collider2D col)
    {
        if (type == 0)
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "anchorObj")
        {
            ExecuteEndLine(col);
        }
        else if(col.gameObject.tag == "triggerNodeAI")
        {
            ExecuteEndLine(col);
            col.gameObject.GetComponent<TriggerNodeAI>().ExecuteTrigger();
        }
    }
}
