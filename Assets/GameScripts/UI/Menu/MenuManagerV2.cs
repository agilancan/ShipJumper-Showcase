using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerV2 : MonoBehaviour
{
    public Transform PanelTransform;
    public GameObject SequencePrefab;
    public GameObject CardPrefab;

    public List<LevelData> LevelDataList = new List<LevelData>();
    public Dictionary<string, GameObject> SequenceGroups = new Dictionary<string, GameObject>();
    public List<Sprite> CardSpriteList = new List<Sprite>();

    private void Start()
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

            string sName = data[i]["World"].ToString() + data[i]["Sequence"].ToString();

            GameObject go;
            if (!SequenceGroups.ContainsKey(sName))
            {
                go = Instantiate(SequencePrefab, PanelTransform);
                MenuSequenceGroup sg = go.GetComponent<MenuSequenceGroup>();
                sg.ID = sName;
                SequenceGroups.Add(sName, go);

                go = Instantiate(CardPrefab, sg.SequencePrefab.transform);
                assignInfoToCard(go, data[i]["Area"].ToString(), data[i]["Level"].ToString(), levelData);
            }
            else
            {
                MenuSequenceGroup sg = SequenceGroups[sName].GetComponent<MenuSequenceGroup>();
                go = Instantiate(CardPrefab, sg.SequencePrefab.transform);
                assignInfoToCard(go, data[i]["Area"].ToString(), data[i]["Level"].ToString(), levelData);
            }
            

            //LevelDataList.Add(levelData);
        }

        
        for(int i = 0;i < 3; i++)
        {
            //go = Instantiate(SequencePrefab, PanelTransform);
            //Instantiate(CardPrefab, go.transform);
        }
        //loadWorldButtons();
    }

    private void assignInfoToCard(GameObject go, string areaName, string levelName, LevelData levelData)
    {
        MenuLevelCard card = go.GetComponent<MenuLevelCard>();
        string areaSub = areaName.Substring(1, 1);
        //Debug.Log(areaSub);
        int areaNum = int.Parse(areaSub);
        //Debug.Log(areaNum);
        card.AreaImage.sprite = CardSpriteList[areaNum - 1];
        card.LevelNameText.text = levelName.Substring(9, levelName.Length - 9);
        card.LevelData = levelData;
    }
}
