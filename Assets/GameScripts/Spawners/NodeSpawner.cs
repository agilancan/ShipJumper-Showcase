using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    public float TimeLeft;
    public float SpawnInterval;

    public GameObject NodeAIPrefab;
    public Vector2 SpawnVelocity;

    public GameObject NodeAI;

    private void createNodeAI()
    {
        NodeAI = Instantiate(NodeAIPrefab, gameObject.transform.position, Quaternion.identity);
        NodeAI.GetComponent<NodeBehaviour>().Velocity = SpawnVelocity;
    }

    void Start()
    {
        TimeLeft = SpawnInterval;
        createNodeAI();
    }

    void Update()
    {
        if(TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
        }
        else
        {
            TimeLeft = SpawnInterval;
            Destroy(NodeAI);
            createNodeAI();
        }
    }
}
