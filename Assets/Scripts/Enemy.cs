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
        if (chromosome == null)
            chromosome = GetComponent<Chromosome>();
    }

    private void Update()
    {
        if (GetComponent<NavMeshAgent>().enabled != true && GetComponent<Rigidbody>().velocity.magnitude < 0.1)
        {
            try
            {
                GetComponent<NavMeshAgent>().enabled = true;
                if (!GetComponent<NavMeshAgent>().isActiveAndEnabled) Destroy(gameObject);
            }
            catch
            {
                Debug.Log("coucou");
            }
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
        Destroy(this.gameObject);
    }
}