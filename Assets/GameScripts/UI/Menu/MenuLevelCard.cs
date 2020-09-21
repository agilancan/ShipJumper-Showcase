using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UnlockedInfo
{
    public GameObject UnlockedObj;
    public Image LevelCompletionOrb;
    public List<Image> OptionalOrbs;
}

[System.Serializable]
public class LockInfo
{
    public GameObject LockedObj;
    public Text UnlockAmount;
}

public class MenuLevelCard : MonoBehaviour
{
    public Image AreaImage;
    public Text LevelNameText;
    public UnlockedInfo UnlockedInfo;
    public LockInfo LockInfo;
    public bool isUnlocked = false;
    public LevelData LevelData;

    private void Awake()
    {
        isUnlocked = false;
        UnlockedInfo.UnlockedObj.SetActive(false);
        LockInfo.LockedObj.SetActive(true);
    }

    

    private void Update()
    {
        if (isUnlocked)
        {
            UnlockedInfo.UnlockedObj.SetActive(true);
            LockInfo.LockedObj.SetActive(false);
        }
        else
        {
            UnlockedInfo.UnlockedObj.SetActive(false);
            LockInfo.LockedObj.SetActive(true);
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(LevelData.Level);
    }
}
