using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public ChainManager ChainManager;
    public GameObject LinkPrefab;
    public GameObject EndLink;
    public GameObject PrevLink;
    public Transform NewLinkSpawn;
    public float LinkWidth;
    public float LineDistance;
    public float MaxLineDistance;

    public List<GameObject> ChainLinkList = new List<GameObject>();

    public bool maxLineReached = false;

    public int ID;

    private void Start()
    {
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
    }

    public void SelfDestruct()
    {
        Destroy(EndLink);
        foreach(GameObject go in ChainLinkList)
        {
            Destroy(go);
        }
        ChainManager.Chains.Remove(this);
        Destroy(gameObject);
    }

    private void FixedUpdate()
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
                        PrevLink.GetComponent<HingeJoint2D>().connectedBody = newPrevLinkRB;
                    }                    
                }
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
}
