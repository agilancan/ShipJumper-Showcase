using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChainType
{
    Normal,
    Connector,
    Swapper,
    MindControl
}
public class Chain : MonoBehaviour
{    
    public ChainType ChainType;

    public ChainManager ChainManager;
    public GameObject LinkPrefab;
    public GameObject EndLink;
    public GameObject StartLink;
    public GameObject SecondLink;
    public GameObject PrevLink;
    public Transform NewLinkSpawn;
    public float LinkWidth;
    public float LineDistance;
    public float MaxLineDistance;
    public bool IsConnected;
    public int ConnectedID;

    public List<GameObject> ChainLinkList = new List<GameObject>();

    public bool maxLineReached = false;

    public int ID;

    public static int ConnectedChainCount = 0;

    private void Start()
    {
        IsConnected = false;
        //LinkWidth = LinkPrefab.GetComponent<CircleCollider2D>().radius/2;
        ChainLinkList.Add(EndLink);
        Link link = EndLink.GetComponent<Link>();
        link.ID = ID;
        link.ChainManager = ChainManager;
    }

    public void ExecuteEndLine()
    {        
        Rigidbody2D prevRB = PrevLink.GetComponent<Rigidbody2D>();
        prevRB.MovePosition(NewLinkSpawn.transform.position);
        PrevLink.GetComponent<HingeJoint2D>().connectedBody = NewLinkSpawn.gameObject.GetComponent<Rigidbody2D>();
        maxLineReached = true;

        if (ChainType == ChainType.Connector)
        {
            Chain otherConnectorChain = null;
            foreach(Chain chain in ChainManager.Chains)
            {
                Debug.Log(chain.ID + " " + chain.ChainType);
                if(chain.ID != ID && chain.ChainType == ChainType.Connector)
                {
                    otherConnectorChain = chain;
                    Debug.Log("OtherChain");
                }
            }
            if(otherConnectorChain != null)
            {
                HingeJoint2D thisStartLinkHinge = StartLink.GetComponent<HingeJoint2D>();
                HingeJoint2D otherStartLinkHinge = otherConnectorChain.StartLink.GetComponent<HingeJoint2D>();

                thisStartLinkHinge.connectedBody = otherConnectorChain.StartLink.GetComponent<Rigidbody2D>();
                //otherStartLinkHinge.SecondLink
                otherStartLinkHinge.connectedBody = StartLink.GetComponent<Rigidbody2D>();

                ConnectedChainCount++;
                otherConnectorChain.IsConnected = true;
                otherConnectorChain.ConnectedID = ConnectedChainCount;
                IsConnected = true;
                ConnectedID = ConnectedChainCount;

                ChainManager.Chains.Remove(this);
                ChainManager.Chains.Remove(otherConnectorChain);
                ChainManager.ConnectedChains.Add(this);
                ChainManager.ConnectedChains.Add(otherConnectorChain);

                //EndLink.GetComponent<Endlink>().AnchorObject.GetComponent<HingeJoint2D>().connectedBody = null;
                //thisStartLinkHinge.connectedBody = EndLink.GetComponent<Rigidbody2D>();

                //thisStartLinkHinge.connectedBody.gameObject.GetComponent<>
                //StartLink.GetComponent
                //StarLink
                //otherConnectorChain.EndLink
                //HingeJoint2D hj2D = StartLink.GetComponent<HingeJoint2D>();
                //hj2D.connectedBody = otherConnectorChain.EndLink.GetComponent<Rigidbody2D>();
            }
        }
    }

    private void normalSelfDestruction()
    {
        Destroy(EndLink);
        foreach (GameObject go in ChainLinkList)
        {
            Destroy(go);
        }
        ChainManager.Chains.Remove(this);
        Destroy(gameObject);
    }
    public void SelfDestruct()
    {
        switch (ChainType)
        {
            case ChainType.Normal:
                normalSelfDestruction();
                break;
            case ChainType.Swapper:
                normalSelfDestruction();
                break;
            case ChainType.MindControl:
                normalSelfDestruction();
                break;
            case ChainType.Connector:
                Chain otherConnectorChain = null;
                foreach (Chain chain in ChainManager.ConnectedChains)
                {
                    Debug.Log(chain.ID + " " + chain.ChainType);
                    if (chain.ID != ID)
                    {
                        otherConnectorChain = chain;
                    }
                }
                if (otherConnectorChain)
                {
                    Destroy(otherConnectorChain.EndLink);
                    foreach (GameObject go in otherConnectorChain.ChainLinkList)
                    {
                        Destroy(go);
                    }
                    ChainManager.ConnectedChains.Remove(otherConnectorChain);
                    Destroy(otherConnectorChain.gameObject);

                    Destroy(EndLink);
                    foreach (GameObject go in ChainLinkList)
                    {
                        Destroy(go);
                    }
                    ChainManager.ConnectedChains.Remove(this);
                    Destroy(gameObject);
                }
                break;
        }        
    }

    private void NormalChainUpdate()
    {
        if (MaxLineDistance > LineDistance && !maxLineReached)
        {
            Rigidbody2D eLinkRB = EndLink.GetComponent<Rigidbody2D>();
            float distance = Vector2.Distance(eLinkRB.position, NewLinkSpawn.position);

            if (LineDistance < distance)
            {
                if (!PrevLink)
                {
                    HingeJoint2D eLinkHinge = EndLink.GetComponent<HingeJoint2D>();                    
                    PrevLink = Instantiate(LinkPrefab, NewLinkSpawn);
                    Link link = PrevLink.GetComponent<Link>();
                    link.ID = ID;
                    link.ChainManager = ChainManager;
                    ChainLinkList.Add(PrevLink);

                    Rigidbody2D prevRB = PrevLink.GetComponent<Rigidbody2D>();
                    eLinkHinge.connectedBody = prevRB;

                    PrevLink.GetComponent<HingeJoint2D>().connectedBody = prevRB;
                }
                else
                {
                    GameObject oldPrevLink = PrevLink;
                    PrevLink = Instantiate(LinkPrefab, NewLinkSpawn);
                    Link link = PrevLink.GetComponent<Link>();
                    link.ID = ID;
                    link.ChainManager = ChainManager;
                    ChainLinkList.Add(PrevLink);
                    Rigidbody2D newPrevLinkRB = PrevLink.GetComponent<Rigidbody2D>();
                    oldPrevLink.GetComponent<HingeJoint2D>().connectedBody = newPrevLinkRB;
                    HingeJoint2D prevHinge2D = PrevLink.GetComponent<HingeJoint2D>();
                    if (prevHinge2D.connectedBody != newPrevLinkRB)
                    {
                        HingeJoint2D hj2D = PrevLink.GetComponent<HingeJoint2D>();
                        hj2D.connectedBody = newPrevLinkRB;
                        hj2D.connectedAnchor = newPrevLinkRB.transform.position;
                        hj2D.anchor = hj2D.gameObject.transform.InverseTransformPoint(
                            newPrevLinkRB.transform.position);
                    }
                }
                SecondLink = StartLink;
                StartLink = PrevLink;
                PrevLink.transform.SetParent(this.transform);
                LineDistance += LinkWidth;
            }
        }
        else
        {
            if (!maxLineReached)
            {
                ExecuteEndLine();
            }
        }
    }

    private void FixedUpdate()
    {
        switch (ChainType)
        {
            case ChainType.Normal:
                NormalChainUpdate();
                break;
            case ChainType.Connector:
                NormalChainUpdate();
                break;
            case ChainType.Swapper:
                NormalChainUpdate();
                break;
            case ChainType.MindControl:
                NormalChainUpdate();
                break;
        }
                
    }
}
