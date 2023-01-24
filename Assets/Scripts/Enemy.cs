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
    public static float AvgLT = long.MaxValue;
    public float lifeTime { get; private set; }
    internal GameObject laser;

    // Start is called before the first frame update
    void Start()
    {
        Chromosome c = gameObject.AddComponent<Chromosome>();
        c.Init();
        if (UnityEngine.Random.Range(0, 10) == 0)
            c.Mutate();
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();/// TODO transfert dans gène 
        if (agent.enabled != true && GetComponent<Rigidbody>().velocity.magnitude < 0.01)
        {
            GetComponent<NavMeshObstacle>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            agent.enabled = true;
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
        if(AvgLT / lifeTime < 1) ltBonus = 0;

        return playerTouched?PLAYER_KILLED_BONUS:0 + AvgLT/lifeTime * ltBonus + GetComponent<Chromosome>().getGenesFitBonus();
    }

    public void Killed()
    {
        gameObject.SetActive(false);
        GetComponentInParent<EnemiesManager>().OnEnemyKilled(this);
    }
}