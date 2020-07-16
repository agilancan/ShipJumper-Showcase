using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentControl : MonoBehaviour
{
    [System.Serializable]
    public class Current 
    {
        public Vector3 CurrentDirection;
        public Vector3 Rotation;
    }

    public Text TimerMessage;
    public GameObject ArrowObj;
    public GameManager GameManager;
    public float TransitionTime;
    public bool IsControlOverride = false;
    public List<Current> CurrentList;

    private int currentIndex = 0;
    private float timeLeft;
    private Player player;
    private NodeBehaviour playerNB;
    void Start()
    {
        player = GameManager.Player;
        playerNB = player.gameObject.GetComponent<NodeBehaviour>();
        timeLeft = TransitionTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsControlOverride)
        {
            
            if (timeLeft >= 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                if(currentIndex >= CurrentList.Count - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    ++currentIndex;
                }
                timeLeft = TransitionTime;
            }
            playerNB.Velocity = CurrentList[currentIndex].CurrentDirection;
            ArrowObj.transform.rotation = Quaternion.Euler(CurrentList[currentIndex].Rotation);
            TimerMessage.text = "Current Change in: " + timeLeft.ToString("0.00");
        }
    }
}
