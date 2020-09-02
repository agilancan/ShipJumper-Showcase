using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public Transform MemberPrefab;
    public Transform EnemyPrefab;
    public int MemberCount;
    public int EnemyCount;
    public List<FlockMember> MemberList = new List<FlockMember>();
    public List<FlockEnemy> EnemyList = new List<FlockEnemy>();
    public float Bounds;
    public float SpawnRadius;

    private void Start()
    {
        spawnMembers();
        spawnEnemies();
    }

    private void spawnMembers()
    {
        for(int i = 0; i < MemberCount; i++)
        {
            MemberList.Add(Instantiate(MemberPrefab, 
                new Vector3(
                    Random.Range(-SpawnRadius, SpawnRadius), 
                    Random.Range(-SpawnRadius, SpawnRadius), 1), 
                Quaternion.identity)
                .GetComponent<FlockMember>());
        }
    }

    private void spawnEnemies()
    {
        for (int i = 0; i < EnemyCount; i++)
        {
            EnemyList.Add(Instantiate(EnemyPrefab,
                new Vector3(
                    Random.Range(-SpawnRadius, SpawnRadius),
                    Random.Range(-SpawnRadius, SpawnRadius), 1),
                Quaternion.identity)
                .GetComponent<FlockEnemy>());
        }
    }

    public List<FlockMember> GetNeighbors(FlockMember member, float radius)
    {
        List<FlockMember> neighborsFound = new List<FlockMember>();
        foreach(FlockMember otherMember in MemberList)
        {
            if (otherMember == member)
                continue;
            if(Vector3.Distance(member.Position, otherMember.Position) <= radius)
            {
                neighborsFound.Add(otherMember);
            }
        }
        return neighborsFound;
    }

    public List<FlockEnemy> GetEnemies(FlockMember member, float radius)
    {
        List<FlockEnemy> returnEnemies = new List<FlockEnemy>();
        foreach (FlockEnemy enemy in EnemyList)
        {
            if(Vector3.Distance(member.Position, enemy.Position) <= radius)
            {
                returnEnemies.Add(enemy);
            }
        }
        return returnEnemies;
    }

}
