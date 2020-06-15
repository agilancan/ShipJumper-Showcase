using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainManager : MonoBehaviour
{
    public ChainCutter ChainCutter;
    public Transform ShipTransform;
    public GameObject ChainGameObject;
    public List<Chain> Chains = new List<Chain>();

    private int chainCount = 1;

    public void CreateChain(Vector2 targetWorldPosition)
    {
        
        GameObject chainObj = Instantiate(ChainGameObject, ShipTransform);
        Chain chain = chainObj.GetComponent<Chain>();
        chain.ChainManager = this;
        chain.ID = chainCount;
        chainCount++;

        Chains.Add(chain);
        
        Vector2 endLinkVelocity = new Vector2(targetWorldPosition.x - chain.EndLink.transform.position.x, targetWorldPosition.y - chain.EndLink.transform.position.y).normalized * 10;
        chainObj.transform.SetParent(ShipTransform);
        chain.NewLinkSpawn = ShipTransform;

        chain.EndLink.GetComponent<Rigidbody2D>().velocity = endLinkVelocity;
    }
}
