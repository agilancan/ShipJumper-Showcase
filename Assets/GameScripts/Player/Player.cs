using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public bool IsPowered;
    public PlayerStatus()
    {
        IsPowered = false;
    }
}

public class Player : MonoBehaviour
{
    public GameManager GameManager;

    public enum Mode
    {
        Normal,
        Launch,
        Launching,
        Swapper
    }
    public PlayerStatus PlayerStatus;
    private NodeBehaviour nodeBehaviour;
    private Rigidbody2D rb;
    [SerializeField]
    private ShipLauncher shipLauncher;

    public Mode CurrentMode;

    public Color BaseColor;
    public SpriteRenderer SR;

    private Vector2 launchVelocity;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        BaseColor = SR.color;
        GameManager = FindObjectOfType<GameManager>();
        nodeBehaviour = GetComponent<NodeBehaviour>();
        rb = GetComponent<Rigidbody2D>();
    }

    public ShipLauncher GetSL()
    {
        return shipLauncher;
    }

    public void ToggleLaunchMode(bool isLaunchMode, ShipLauncher sl)
    {
        if (isLaunchMode)
        {
            CurrentMode = Mode.Launch;
            nodeBehaviour.IsOverrideEnabled = true;
            nodeBehaviour.VelocityOverride = Vector3.zero;
            shipLauncher = sl;
            rb.MovePosition(sl.gameObject.transform.position);
        }
        else
        {
            CurrentMode = Mode.Normal;
            nodeBehaviour.IsOverrideEnabled = false;
            nodeBehaviour.VelocityOverride = Vector3.zero;
            shipLauncher = null;
        }
    }

    private void Update()
    {
        switch (CurrentMode)
        {
            case Mode.Normal:
                if (PlayerStatus.IsPowered)
                {
                    SR.color = Color.yellow;
                }
                else
                {
                    SR.color = BaseColor;
                }
                break;
            case Mode.Swapper:
                SR.color = Color.black;
                break;
            case Mode.Launch:
                break;
            case Mode.Launching:
                break;
        }
    }
}
