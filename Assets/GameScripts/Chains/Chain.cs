using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChainType
{
    Normal,
    Swapper
}

[System.Serializable]
public class ChainStatus
{
    public bool IsPowered;
    public ChainStatus()
    {
        IsPowered = false;
    }

}
public class Chain : MonoBehaviour
{    
    public ChainType ChainType;
    public ChainStatus ChainStatus;

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

    public LineRenderer ChainLineRenderer;
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
        ChainLineRenderer = GetComponent<LineRenderer>();
        link.ID = ID;
        link.Chain = this;
        link.ChainManager = ChainManager;
    }

    public void ExecuteEndLine()
    {        
        Rigidbody2D prevRB = PrevLink.GetComponent<Rigidbody2D>();
        prevRB.MovePosition(NewLinkSpawn.transform.position);
        PrevLink.GetComponent<HingeJoint2D>().connectedBody = NewLinkSpawn.gameObject.GetComponent<Rigidbody2D>();
        maxLineReached = true;
    }

    private void normalSelfDestruction()
    {
        Endlink endlink = EndLink.GetComponent<Endlink>();
        if (endlink.AnchorObject)
        {
            Power power = endlink.AnchorObject.GetComponent<Power>();
            SharkAttractor sa = endlink.AnchorObject.GetComponent<SharkAttractor>();
            
            if (power)
            {
                power.Disconnect();
            }
            if (sa)
            {
                if (sa.IsActive())
                {
                    sa.Deactivate();
                }                
            }
            
        }
        
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
                    link.Chain = this;
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
                    link.Chain = this;
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

        ChainLineRenderer.positionCount = ChainLinkList.Count;
    }

    private void FixedUpdate()
    {
        switch (ChainType)
        {
            case ChainType.Normal:
                NormalChainUpdate();
                break;
            case ChainType.Swapper:
                NormalChainUpdate();
                break;
        }

        for(int i = 0; i < ChainLineRenderer.positionCount; i++)
        {
            ChainLineRenderer.SetPosition(i, ChainLinkList[i].transform.position);
        }
                
    }
}
