using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    ST_NodeBehaviour
}

[System.Serializable]
public class SpawnInfo
{
    public Vector2 SpawnVelocity;
}

public class IntervalSpawner : MonoBehaviour
{
    public GameObject SpawnObjectPrefab;
    public float SpawnObjectInterval;
    public float ObjectDeathTime;
    
    public SpawnType SpawnType;
    public SpawnInfo SpawnInfo;

    private Dictionary<GameObject, float> ObjectDeathTimers = new Dictionary<GameObject, float>();
    private float spawnObjectIntervalTimeLeft;
    private List<GameObject> garbageList = new List<GameObject>();

    private void spawnObject()
    {
        GameObject go = Instantiate(SpawnObjectPrefab, gameObject.transform.position, Quaternion.identity);
        // Hack for pufferfish rotation
        if(SpawnInfo.SpawnVelocity.x > 0)
        {
            go.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        ObjectDeathTimers.Add(go, ObjectDeathTime);
        switch (SpawnType)
        {
            case SpawnType.ST_NodeBehaviour:
                go.GetComponent<NodeBehaviour>().Velocity = SpawnInfo.SpawnVelocity;
                break;
        }
    }

    private void Start()
    {
        spawnObjectIntervalTimeLeft = SpawnObjectInterval;
        spawnObject();
    }

    private void Update()
    {
        if(ObjectDeathTimers.Count > 0)
        {
            foreach (GameObject go in ObjectDeathTimers.Keys.ToList())
            {
                if (ObjectDeathTimers[go] > 0)
                {
                    ObjectDeathTimers[go] -= Time.deltaTime;
                }
                else
                {
                    garbageList.Add(go);
                }
            }
        }        

        if(garbageList.Count > 0)
        {
            foreach (GameObject go in garbageList)
            {
                ObjectDeathTimers.Remove(go);
                Destroy(go);
            }
            garbageList.Clear();
        }
        
        if (spawnObjectIntervalTimeLeft > 0)
        {
            spawnObjectIntervalTimeLeft -= Time.deltaTime;
        }
        else
        {
            spawnObjectIntervalTimeLeft = SpawnObjectInterval;
            spawnObject();
        }
    }
}
