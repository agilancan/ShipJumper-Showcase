using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockEnemy : FlockMember
{
    protected override Vector3 Combine()
    {
        return Config.WanderPriority * Wander();
    }
}
