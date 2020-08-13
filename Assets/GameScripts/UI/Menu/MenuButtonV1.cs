using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonV1 : MonoBehaviour
{    
    public Text ButtonText;
    public int LoadSceneType;
    public LevelData LevelData;

    private MenuManager menuManager;

    private void Awake()
    {
        menuManager = FindObjectOfType<MenuManager>();
    }
    public void OnButtonClick()
    {
        menuManager.Load(LevelData, LoadSceneType);
    }
}
