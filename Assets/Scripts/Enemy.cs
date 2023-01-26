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
    internal Transform aim;
    const int PLAYER_KILLED_BONUS = 50;
    static int DISTANCE_BONUS = 10;
    bool playerTouched = false;
    public float popUpDist = -1;
    float distanceToPlayer = int.MaxValue;
    public static float AvgLT = long.MaxValue;
    public float LifeTime { get; private set; }
    public GameObject laser;
    internal Enemy[] parents;

    // Start is called before the first frame update
    void Start()
    {
        Chromosome c = gameObject.AddComponent<Chromosome>();
        c.Init();
        if (parents != null)
            c.Crossover(parents[0].GetComponent<Chromosome>(), parents[1].GetComponent<Chromosome>());
        if (UnityEngine.Random.Range(0, 10) == 0)
            c.Mutate();
    }

    private void Update()
    {
        LifeTime += Time.deltaTime;
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
            }
        }
        distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);

        if (distanceToPlayer < 1) 
            GetComponent<FlyGene>().enabled = false;
        else  
            GetComponent<FlyGene>().enabled = true;
        transform.LookAt(Player.transform);

        if (transform.position.y < -20 || LifeTime > 40)
        {
            Killed();
        }
    }

    public float Fitness()
    {
        return (playerTouched?PLAYER_KILLED_BONUS:0) + DISTANCE_BONUS * popUpDist/LifeTime + GetComponent<Chromosome>().getGenesFitBonus();
    }

    public void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject == Player.GetComponentInParent<Player>().gameObject || collision.gameObject.GetComponent<PlayerHead>() != null)
        {
           GameObject.Find("HeadCollider").GetComponent<PlayerHead>().Touched(this);
            playerTouched = true;
        }
    }

    public void Killed()
    {
        gameObject.SetActive(false);
        GetComponentInParent<EnemiesManager>().OnEnemyKilled(this);
        Debug.Log(ToString());
    }

    public override string ToString()
    {
        string desc = "Killed : ";
        foreach (Gene g in GetComponent<Chromosome>().GetGenes().Values)
        {
            desc += g.type + " : " + g.proportionnalValue() + ", "; 
        }
        desc += "total : " + GetComponent<Chromosome>().totalWeight;
        return desc;
    }
}