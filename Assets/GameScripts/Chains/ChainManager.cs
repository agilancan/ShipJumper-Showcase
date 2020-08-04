using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainManager : MonoBehaviour
{
    public ChainCutter ChainCutter;
    public Transform ShipTransform;
    public List<GameObject> ChainPrefabs;
    public List<Chain> Chains = new List<Chain>();
    public List<Chain> ConnectedChains = new List<Chain>();

    private GameManager gameManager;
    private int chainCount = 1;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.ChainManager = this;
        ChainCutter = FindObjectOfType<ChainCutter>();
        ShipTransform = FindObjectOfType<Player>().gameObject.transform;
    }

    private GameObject getChainPrefab(ChainType ct)
    {
        return ChainPrefabs.Find(go => go.GetComponent<Chain>().ChainType == ct);
    }

    public void CreateChain(Vector2 targetWorldPosition, ChainType ct)
    {
        if (!gameManager.Player.IsMindControlling)
        {
            GameObject chainObj = Instantiate(getChainPrefab(ct), ShipTransform);
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

    public void CutAllChains()
    {
        Endlink endlink;
        foreach (Chain chain in Chains)
        {
            endlink = chain.EndLink.GetComponent<Endlink>();
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
                    sa.Deactivate();
                }
            }
            
            Destroy(chain.EndLink);
            foreach(GameObject go in chain.ChainLinkList)
            {
                Destroy(go);
            }
            Destroy(chain.gameObject);
        }
        Chains.Clear();
    }
}
