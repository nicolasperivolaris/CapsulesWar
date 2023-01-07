using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR.InteractionSystem;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    private Chromosome chromosome;
    int playerTouched = 0;
    long lifeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetChromosome(Chromosome chromosome)
    {
        this.chromosome = chromosome;
        foreach (var gene in chromosome)
        {
            gene.Value.Express(this);
        }
    }

    private void Update()
    {
        if (GetComponent<NavMeshAgent>().enabled != true && GetComponent<Rigidbody>().velocity.magnitude < 0.1)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            if (!GetComponent<NavMeshAgent>().isActiveAndEnabled) Destroy(gameObject);
        }
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent.isOnNavMesh) agent.SetDestination(Player.transform.position);
    }

    public float Fitness()
    {
        return playerTouched/lifeTime;
    }

    public void Killed()
    {
        GetComponentInParent<EnemiesManager>().OnEnemyKilled(this);
        Destroy(gameObject);
    }
}