using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private ChainCutter chainCutter;
    private GameManager gameManager;
    private ChainManager chainManager;
    private Player player;

    private float lauchTimeLeft = 0;
    private float currentForce;
    private Vector2 direction;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        FindObjectOfType<GameManager>().InputController = this;
        player = FindObjectOfType<Player>();
        chainManager = FindObjectOfType<ChainManager>();
        chainCutter = FindObjectOfType<ChainCutter>();
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

    private void PlayerInput(bool isDoubleTap, ChainType ct)
    {
        Vector3 targetWorldPosition;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            targetWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);
            if (isDoubleTap && chainManager.Chains.Count < 3)
            {
                if (!player.IsMindControlling)
                {
                    chainManager.CreateChain(targetWorldPosition, ct);
                }
                else
                {
                    direction = (targetWorldPosition - player.transform.position).normalized;
                    player.MindControlledNode.VelocityOverride = direction * 2;
                    lauchTimeLeft = 1;
                    currentForce = 2;
                }
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    break;
                case TouchPhase.Moved:
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
        else
        {
            targetWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (!gameManager.Player.IsMindControlling)
                {
                    chainManager.CreateChain(targetWorldPosition, ct);
                }
                else
                {

                }                    
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
        if (lauchTimeLeft >= 0)
        {
            lauchTimeLeft -= Time.deltaTime;
            currentForce -= 0.01f;
            if (currentForce >= 0)
            {
                player.MindControlledNode.VelocityOverride = direction * currentForce;
            }
        }
        else
        {
            if (player.MindControlledNode)
            {
                player.MindControlledNode.IsOverrideEnabled = false;
                player.MindControlledNode.VelocityOverride = Vector3.zero;
                player.MindControlledNode = null;
            }            
        }
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
        switch (player.CurrentMode)
        {
            case Player.Mode.Normal:
                PlayerInput(isDoubleTap, ChainType.Normal);
                break;
            case Player.Mode.Launch:
                ShipLauncher sl = player.GetSL();
                if (sl)
                {
                    Vector3 targetWorldPosition;
                    if (Input.touchCount > 0 && isDoubleTap)
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
            case Player.Mode.Connector:
                PlayerInput(isDoubleTap, ChainType.Connector);
                break;
            case Player.Mode.Swapper:
                PlayerInput(isDoubleTap, ChainType.Swapper);
                break;
            case Player.Mode.MindControl:
                PlayerInput(isDoubleTap, ChainType.MindControl);
                break;
        }
        
    }
}
