using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    public int MoveSpeed = 10;
    public Chromosome chromosome;
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
        GetComponent<NavMeshAgent>().destination = Player.transform.position;
    }

    public int Fitness()
    {
        return 0;
    }
}