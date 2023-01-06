using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR.InteractionSystem;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    public Chromosome chromosome;
    int playerTouched = 0;
    long lifeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = Player.transform.position;
        if (chromosome == null)
            chromosome = GetComponent<Chromosome>();
    }

    private void FixedUpdate()
    {
        GetComponent<NavMeshAgent>().SetDestination(Player.transform.position);
    }

    public float Fitness()
    {
        return playerTouched/lifeTime;
    }

    public void Killed()
    {
        GetComponentInParent<EnemiesManager>().OnEnemyKilled(this);
        Destroy(this.gameObject);
    }
}