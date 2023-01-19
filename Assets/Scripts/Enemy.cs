using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
using Valve.VR.InteractionSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    const int PLAYER_KILLED_BONUS = 50;
    const int LTBonus = 20;
    bool playerTouched = false;
    float distanceToPlayer = int.MaxValue;
    public static long AvgLT = long.MaxValue;
    long birthTime = 0;
    long deathTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        birthTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }

    public void Jump()
    {
        if (GetComponent<NavMeshAgent>().isActiveAndEnabled)
        {
            GetComponent<NavMeshObstacle>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
            Vector3 rbV = GetComponent<Rigidbody>().velocity;
            Vector3 nmaV = GetComponent<NavMeshAgent>().velocity;
            GetComponent<Rigidbody>().velocity = new Vector3(nmaV.x, nmaV.y + 5, nmaV.z) + GetComponent<NavMeshAgent>().transform.forward;
        }
    }

    private void Update()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent.enabled != true && GetComponent<Rigidbody>().velocity.magnitude < 0.01)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<NavMeshObstacle>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            if(agent.isOnNavMesh) 
                agent.SetDestination(Player.transform.position);
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

    public class SpeedGene:Gene
    {

        public SpeedGene() : base(SPEED, 1, 1)
        {}

        public override int getFitBonus()
        {
            if (GetComponentInParent<Enemy>().distanceToPlayer < 3) return 50;
            else return 0;
        }

        public void Update()
        {
            GetComponentInParent<NavMeshAgent>().speed = value / (float)(GetComponentInParent<Chromosome>().totalWeight);
        }
    }
}