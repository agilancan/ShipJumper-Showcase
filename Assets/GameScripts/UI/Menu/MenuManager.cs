using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelData
{
    public string World;
    public string Sequence;
    public string Level;
    public LevelData(string world, string sequence, string level)
    {
        World = world;
        Sequence = sequence;
        Level = level;
    }
}

public class MenuManager : MonoBehaviour
{
    public Transform ButtonContentList;
    public GameObject ButtonPrefab;
    public GameObject PreviousButton;

    public List<LevelData> LevelDataList = new List<LevelData>();
    public List<GameObject> ButtonList = new List<GameObject>();
    public LevelData CurrentSceneLevelData;

    public int SceneID;

    private void Awake()
    {
        loadWorldButtons();
        List<Dictionary<string, object>> data = CSVReader.Read("LevelData");
        for(int i = 0; i < data.Count; i++)
        {
            LevelDataList.Add(new LevelData(
                data[i]["World"].ToString(),
                data[i]["Sequence"].ToString(), 
                data[i]["Level"].ToString()));
        }
    }

    private void loadWorldButtons()
    {
        CurrentSceneLevelData.World = null;
        CurrentSceneLevelData.Sequence = null;
        CurrentSceneLevelData.Level = null;
        SceneID = 0;
        destroyButtons();
        GameObject go;
        for(int i = 1; i <= 5; i++)
        {
            go = Instantiate(ButtonPrefab, ButtonContentList);            
            ButtonList.Add(go);
            MenuButtonV1 mb = go.GetComponent<MenuButtonV1>();
            mb.LoadSceneType = 1;
            mb.LevelData.World = i.ToString();
            mb.ButtonText.text = "World " + i;
        }
    }

    private void LoadSequences(int world)
    {
        CurrentSceneLevelData.World = world.ToString();
        CurrentSceneLevelData.Sequence = null;
        CurrentSceneLevelData.Level = null;
        SceneID = 1;
        destroyButtons();
        int sCount = 3;
        if (world > 3)
        {
            sCount = 2;            
        }
        GameObject go;
        for (int i = 1; i <= sCount; i++)
        {
            go = Instantiate(ButtonPrefab, ButtonContentList);
            ButtonList.Add(go);
            MenuButtonV1 mb = go.GetComponent<MenuButtonV1>();
            mb.LoadSceneType = 2;
            mb.LevelData.World = world.ToString();
            mb.LevelData.Sequence = i.ToString();
            mb.ButtonText.text = "Sequence " + i;
        }
    }

    private void LoadLevels(int world, int sequence)
    {
        CurrentSceneLevelData.World = world.ToString();
        CurrentSceneLevelData.Sequence = sequence.ToString();
        CurrentSceneLevelData.Level = null;
        SceneID = 2;
        destroyButtons();
        var levels = LevelDataList.Where(
            ld => ld.World == world.ToString() && ld.Sequence == sequence.ToString());
        GameObject go;
        foreach (LevelData ld in levels)
        {
            go = Instantiate(ButtonPrefab, ButtonContentList);
            ButtonList.Add(go);
            MenuButtonV1 mb = go.GetComponent<MenuButtonV1>();
            mb.LoadSceneType = 3;
            mb.LevelData.World = ld.World;
            mb.LevelData.Sequence = ld.Sequence;
            mb.LevelData.Level = ld.Level;
            mb.ButtonText.text = ld.Level;
        }
    }

    private void destroyButtons()
    {
        foreach(GameObject go in ButtonList)
        {
            Destroy(go);
        }
        ButtonList.Clear();
    }

    private void Update()
    {
        if(SceneID == 0)
        {
            PreviousButton.SetActive(false);
        }
        else
        {
            PreviousButton.SetActive(true);
        }
    }

    public void Load(LevelData levelData, int sceneType)
    {
        switch (sceneType)
        {
            case 0:
                loadWorldButtons();
                break;
            case 1:
                LoadSequences(int.Parse(levelData.World));
                break;
            case 2:
                LoadLevels(int.Parse(levelData.World), int.Parse(levelData.Sequence));
                break;
            case 3:
                SceneManager.LoadScene(levelData.Level);
                break;
        }
    }
    
    public void LoadPrevious()
    {
        switch (SceneID)
        {
            case 1:
                loadWorldButtons();
                break;
            case 2:
                LoadSequences(int.Parse(CurrentSceneLevelData.World));
                break;
        }
    }
}
