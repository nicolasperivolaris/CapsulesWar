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
            gene.Value.Start(this);
        }
    }

    public void Jump()
    {
        GetComponent<NavMeshAgent>().Move(GetComponent<NavMeshAgent>().transform.forward * 2);

        /*
        Vector3 dir = GetComponent<NavMeshAgent>().transform.forward;
        GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = dir;*/
    }

    private void Update()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent.enabled != true && GetComponent<Rigidbody>().velocity.magnitude < 0.01)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            if(agent.isOnNavMesh) 
                agent.SetDestination(Player.transform.position);
        }

        foreach (Gene gene in chromosome.Values)
        {
            gene.Update(this); 
        }

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