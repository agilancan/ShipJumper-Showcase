using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMember : MonoBehaviour
{
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 Acceleration;

    public FlockManager FlockManager;
    public FlockMemberConfig Config;

    private Vector3 wanderTarget;

    private void Start()
    {
        FlockManager = FindObjectOfType<FlockManager>();
        Config = FindObjectOfType<FlockMemberConfig>();

        Position = transform.position;
        Velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
    }

    private void Update()
    {
        Acceleration = Combine();
        Acceleration = Vector3.ClampMagnitude(Acceleration, Config.MaxAcceleration);
        Velocity = Velocity + Acceleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, Config.MaxVelocity);
        transform.rotation = Quaternion.LookRotation(Velocity);
        Position = Position + Velocity * Time.deltaTime;
        wrapAround(ref Position, -FlockManager.Bounds, FlockManager.Bounds);
        transform.position = Position;
    }

    protected Vector3 Wander()
    {
        float jitter = Config.WanderJitter * Time.deltaTime;
        wanderTarget += new Vector3(randomBinomial() * jitter, randomBinomial() * jitter, 0);
        wanderTarget = wanderTarget.normalized;
        wanderTarget *= Config.WanderRadius;
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(Config.WanderDistance, Config.WanderDistance, 0);
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
        targetInWorldSpace -= this.Position;
        return targetInWorldSpace.normalized;
    }

    protected Vector3 Cohesion()
    {
        Vector3 cohesionVector = new Vector3();
        int countMembers = 0;
        var neighbors = FlockManager.GetNeighbors(this, Config.CohesionRadius);
        if (neighbors.Count == 0)
            return cohesionVector;
        foreach(var member in neighbors)
        {
            if (isInFOV(member.Position))
            {
                cohesionVector += member.Position;
                countMembers++;
            }
        }
        if (countMembers == 0)
            return cohesionVector;

        cohesionVector /= countMembers;
        cohesionVector = cohesionVector - this.Position;
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;
    }

    protected Vector3 Alignment()
    {
        Vector3 alignVector = new Vector3();
        var members = FlockManager.GetNeighbors(this, Config.AlignmentRadius);
        if (members.Count == 0)
            return alignVector;

        foreach(var member in members)
        {
            if (isInFOV(member.Position))
                alignVector += member.Velocity;
        }
        return alignVector.normalized;
    }

    protected Vector3 Seperation()
    {
        Vector3 seperateVector = new Vector3();
        var members = FlockManager.GetNeighbors(this, Config.SeperationRadius);
        if (members.Count == 0)
            return seperateVector;
        foreach(var member in members)
        {
            if (isInFOV(member.Position))
            {
                Vector3 movingTowards = this.Position - member.Position;
                if(movingTowards.magnitude > 0)
                {
                    seperateVector += movingTowards.normalized / movingTowards.magnitude;
                }
            }
        }
        return seperateVector.normalized;
    }

    protected Vector3 Avoidance()
    {
        Vector3 avoidanceVector = new Vector3();
        var enemyList = FlockManager.GetEnemies(this, Config.AvoidanceRadius);
        if (enemyList.Count == 0)
            return avoidanceVector;
        foreach(var enemy in enemyList)
        {
            avoidanceVector += RunAway(enemy.Position);
        }
        return avoidanceVector.normalized;
    }

    private Vector3 RunAway(Vector3 target)
    {
        Vector3 neededVelocity = (Position - target).normalized * Config.MaxVelocity;
        return neededVelocity;
    }

    virtual protected Vector3 Combine()
    {
        Vector3 finalVec =
            Config.CohesionPriority * Cohesion()
            + Config.WanderPriority * Wander()
            + Config.AlignmentPriority * Alignment()
            + Config.SeperationPriority * Seperation()
            + Config.AvoidancePriority * Avoidance();
        return finalVec;
    }

    private void wrapAround(ref Vector3 vector, float min, float max)
    {
        vector.x = wrapAroundFloat(vector.x, min, max);
        vector.y = wrapAroundFloat(vector.y, min, max);
        vector.z = wrapAroundFloat(vector.z, min, max);
    }

    private float wrapAroundFloat(float value, float min, float max)
    {
        if (value > max)
        {
            value = min;
        }            
        else if (value < min)
        {
            value = max;
        }            
        return value;
    }

    private float randomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    private bool isInFOV(Vector3 vec)
    {
        return Vector3.Angle(this.Velocity, vec - this.Position) <= Config.MaxFov;
    }
}
