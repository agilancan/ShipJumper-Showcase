using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area
{
    public string Name;
    public GameObject Prefab;
}

public class AreaLoader : MonoBehaviour
{
    
    public List<Area> Areas;
    public Dictionary<string, GameObject> AreaPrefabs = new Dictionary<string, GameObject>();

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        foreach (Area area in Areas)
        {
            AreaPrefabs.Add(area.Name, area.Prefab);
        }
    }

    private void Start()
    {
        if(LevelData.AllLevelData.Count <= 0)
        {
            List<Dictionary<string, object>> data = CSVReader.Read("LevelData");
            LevelData levelData;
            for (int i = 0; i < data.Count; i++)
            {
                levelData = new LevelData(
                    data[i]["World"].ToString(),
                    data[i]["Sequence"].ToString(),
                    data[i]["Level"].ToString(),
                    data[i]["Area"].ToString());
                LevelData.AllLevelData.Add(data[i]["Level"].ToString(), levelData);
            }
        }
        Instantiate(AreaPrefabs[LevelData.AllLevelData[gameManager.CurrentLevel].Area]);
    }
}
