using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endlink : MonoBehaviour
{
    public GameObject FirstLink;
    public Chain Chain;
    public int type;
    public GameObject AnchorObject;
    static public HingeJoint2D HingeJoint;
    static public HingeJoint2D HingeJoint2;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();;
    }

    void ExecuteEndLine(Collider2D col)
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(col.gameObject.transform.position);
        if (type == 0)
        {
            AnchorObject = col.gameObject;
            HingeJoint = AnchorObject.GetComponent<HingeJoint2D>();
            HingeJoint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
            Vector3 objLocalPosition = col.gameObject.transform.InverseTransformPoint(gameObject.transform.position);
            HingeJoint.connectedAnchor = gameObject.transform.position;
            HingeJoint.anchor = objLocalPosition;
            Chain.ExecuteEndLine();
        }
        else
        {
            AnchorObject = col.gameObject;
            HingeJoint2 = AnchorObject.GetComponent<HingeJoint2D>();
            HingeJoint2.connectedBody = gameObject.GetComponent<Rigidbody2D>();
            Vector3 objLocalPosition = col.gameObject.transform.InverseTransformPoint(gameObject.transform.position);
            HingeJoint2.connectedAnchor = gameObject.transform.position;
            HingeJoint2.anchor = objLocalPosition;
            Chain.ExecuteEndLine();
        }        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!Chain.IsConnected)
        {
            SharkAttractor sa = col.gameObject.GetComponent<SharkAttractor>();
            Swapper swapper = col.gameObject.GetComponent<Swapper>();
            if (sa && Chain.ChainType != ChainType.Swapper)
            {
                sa.Activate(Chain);
            }
            Power power = col.gameObject.GetComponent<Power>();            
            if (swapper)
            {
                swapper.ActivateSwapperMode();
            }
            if (power && Chain.ChainType != ChainType.Swapper)
            {
                power.Connect();
            }
            if (col.gameObject.tag == "anchorObj")
            {
                if (Chain.ChainType == ChainType.Swapper && !swapper)
                {
                    Chain.ExecuteEndLine();
                    gameManager.ChainManager.CutAllChains();
                    Vector2 nodePos = col.gameObject.transform.position;
                    col.gameObject.transform.position = gameManager.Player.gameObject.transform.position;
                    gameManager.Player.gameObject.transform.position = nodePos;                    
                    gameManager.Player.CurrentMode = Player.Mode.Normal;
                    gameManager.Player.SR.color = gameManager.Player.BaseColor;
                }
                else
                {
                    ExecuteEndLine(col);
                    Chain.IsConnected = true;
                }
            }
            else if (col.gameObject.tag == "triggerNodeAI")
            {
                if (Chain.ChainType == ChainType.Swapper)
                {
                    Chain.ExecuteEndLine();
                    Chain.SelfDestruct();
                    Vector2 nodePos = col.gameObject.transform.position;
                    col.gameObject.transform.position = gameObject.transform.position;
                    gameObject.transform.position = nodePos;
                }
                else
                {
                    ExecuteEndLine(col);
                    Chain.IsConnected = true;
                    col.gameObject.GetComponent<TriggerNodeAI>().ExecuteTrigger();
                }
            }
        }        
    }
}
