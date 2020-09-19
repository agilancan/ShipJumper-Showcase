using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float SingleTapWaitTime = 0.01f;
    private ChainCutter chainCutter;
    private GameManager gameManager;
    private ChainManager chainManager;
    private Player player;

    private float lauchTimeLeft = 0;
    private float currentForce;
    private Vector2 direction;

    private bool singleTapWaitInProcess = false;
    private float singleTapWaitTimeLeft;
    private ChainType singleTapChainType;
    private Vector3 targetWorldPosition;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        FindObjectOfType<GameManager>().InputController = this;
        player = FindObjectOfType<Player>();
        chainManager = FindObjectOfType<ChainManager>();
        chainCutter = FindObjectOfType<ChainCutter>();
        singleTapWaitTimeLeft = SingleTapWaitTime;
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

    private void PlayerInput(ChainType ct)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            targetWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if(Input.touchCount == 2)
            {
                if (gameManager.IsGamePaused)
                {
                    gameManager.UIManager.PauseMenu.SetActive(false);
                    gameManager.ResumeGame();
                }
                else
                {
                    gameManager.UIManager.PauseMenu.SetActive(true);
                    gameManager.PauseGame();
                }
            }
            else if(!gameManager.IsGamePaused)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if(chainManager.Chains.Count < 4)
                        {
                            singleTapWaitInProcess = true;
                            singleTapWaitTimeLeft = SingleTapWaitTime;
                            singleTapChainType = ct;
                        }                        
                        break;
                    case TouchPhase.Moved:
                        singleTapWaitInProcess = false;
                        if (!chainCutter.IsCutting())
                        {
                            chainCutter.StartCutting(targetWorldPosition);
                        }
                        break;
                    case TouchPhase.Canceled:
                        break;
                    case TouchPhase.Ended:
                        chainCutter.StopCutting();
                        break;
                }
            }            
        }
        else
        {
            targetWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gameManager.IsGamePaused)
                {
                    gameManager.UIManager.PauseMenu.SetActive(false);
                    gameManager.ResumeGame();
                }
                else
                {
                    gameManager.UIManager.PauseMenu.SetActive(true);
                    gameManager.PauseGame();
                }
                
            }
            if (Input.GetMouseButtonDown(0))
            {
                chainManager.CreateChain(targetWorldPosition, ct);
            }
            if (Input.GetMouseButtonDown(1))
            {
                chainCutter.StartCutting(targetWorldPosition);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                chainCutter.StopCutting();
            }
        }
        chainCutter.UpdateCut(targetWorldPosition);
    }

    void Update()
    {
        if (singleTapWaitInProcess)
        {
            singleTapWaitTimeLeft -= Time.deltaTime;
            if(singleTapWaitTimeLeft <= 0)
            {
                singleTapWaitInProcess = false;
                chainManager.CreateChain(targetWorldPosition, singleTapChainType);
            }
        }
        if (lauchTimeLeft >= 0)
        {
            lauchTimeLeft -= Time.deltaTime;
            currentForce -= 0.01f;
        }
        switch (player.CurrentMode)
        {
            case Player.Mode.Normal:
                PlayerInput(ChainType.Normal);
                break;
            case Player.Mode.Launch:
                ShipLauncher sl = player.GetSL();
                if (sl)
                {
                    Vector3 targetWorldPosition;
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);
                        targetWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        sl.LaunchPLayer(targetWorldPosition);
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            targetWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            sl.LaunchPLayer(targetWorldPosition);
                        }
                    }
                }
                
                break;
            case Player.Mode.Launching:
                break;
            case Player.Mode.Swapper:
                PlayerInput(ChainType.Swapper);
                break;
        }
        
    }
}
