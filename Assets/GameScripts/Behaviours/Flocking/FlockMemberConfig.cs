using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMemberConfig : MonoBehaviour
{
    public float MaxFov = 180;
    public float MaxAcceleration;
    public float MaxVelocity;

    // Wander
    public float WanderJitter;
    public float WanderRadius;
    public float WanderDistance;
    public float WanderPriority;

    // Cohesion
    public float CohesionRadius;
    public float CohesionPriority;

    // Alignment
    public float AlignmentRadius;
    public float AlignmentPriority;

    // Seperation
    public float SeperationRadius;
    public float SeperationPriority;

    // Avoidance
    public float AvoidanceRadius;
    public float AvoidancePriority;
}
