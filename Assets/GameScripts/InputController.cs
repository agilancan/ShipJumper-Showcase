using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameManager GameManager;
    public ChainCutter ChainCutter;
    private ChainManager chainManager;

    void Start()
    {
        chainManager = GameManager.ChainManager;
    }

    public bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 1;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;
        }
        return result;
    }

    void Update()
    {
        bool isDoubleTap = false;

        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (Input.GetTouch(i).tapCount == 2)
                {
                    isDoubleTap = true;
                }
            }
        }
        Vector3 targetWorldPosition;
        if (Input.touchCount > 0)
        {            
            Touch touch = Input.GetTouch(0);
            targetWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);
            if (isDoubleTap && chainManager.Chains.Count < 3)
            {                
                chainManager.CreateChain(targetWorldPosition);
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    break;
                case TouchPhase.Moved:
                    if (!ChainCutter.IsCutting())
                    {
                        ChainCutter.StartCutting(targetWorldPosition);
                    }                    
                    break;
                case TouchPhase.Canceled:
                    break;
                case TouchPhase.Ended:
                    ChainCutter.StopCutting();
                    break;
            }
        }
        else
        {
            targetWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {                
                chainManager.CreateChain(targetWorldPosition);
            }
            if (Input.GetMouseButtonDown(1))
            {
                ChainCutter.StartCutting(targetWorldPosition);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                ChainCutter.StopCutting();
            }            
        }
        ChainCutter.UpdateCut(targetWorldPosition);
    }
}
