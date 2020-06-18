using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager GameManager;
    public enum Mode
    {
        Normal,
        Launch,
        Launching
    }
    private NodeBehaviour nodeBehaviour;
    private Rigidbody2D rb;
    [SerializeField]
    private ShipLauncher shipLauncher;

    public Mode CurrentMode;

    private Vector2 launchVelocity;

    private void Start()
    {
        CurrentMode = Mode.Normal;
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
                break;
            case Mode.Launch:
                break;
            case Mode.Launching:
                break;
        }
    }
}
