using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
using Valve.VR.InteractionSystem;
using static UnityEngine.EventSystems.EventTrigger;

public partial class Enemy : MonoBehaviour
{
    public GameObject Player;
    const int PLAYER_KILLED_BONUS = 50;
    const int LTBonus = 20;
    bool playerTouched = false;
    float distanceToPlayer = int.MaxValue;
    public static long AvgLT = long.MaxValue;
    long birthTime = 0;
    long deathTime = 0;
    internal GameObject laser;

    // Start is called before the first frame update
    void Start()
    {
        birthTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }

    private void Update()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();/// TODO transfert dans gène 
        if (agent.enabled != true && agent.isOnNavMesh && GetComponent<Rigidbody>().velocity.magnitude < 0.01)
        {
            GetComponent<NavMeshObstacle>().enabled = false;
            agent.enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(Player.transform.position);
                transform.LookAt(Player.transform);
            }
        }
        distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
    }

    public float Fitness()
    {
        float ltBonus = LTBonus;
        if(AvgLT / getLifeTime() < 1) ltBonus = 0;

        return playerTouched?PLAYER_KILLED_BONUS:0 + AvgLT/getLifeTime() * ltBonus + GetComponent<Chromosome>().getGenesFitBonus();
    }

    public void Killed()
    {
        deathTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        GetComponentInParent<EnemiesManager>().OnEnemyKilled(this);
        Destroy(gameObject);
    }

    public long getLifeTime()
    {
        if(deathTime > 0) return deathTime-birthTime;
        else return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - birthTime;
    }
}