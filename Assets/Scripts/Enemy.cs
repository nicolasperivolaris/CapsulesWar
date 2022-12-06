using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject Player;
    public int MoveSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = Player.transform.position;
    }

    private void FixedUpdate()
    {
        GetComponent<NavMeshAgent>().destination = Player.transform.position;
    }
    /*
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            transform.LookAt(Player.transform);
            Debug.Log("Did Hit");
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player.transform);
        if (Vector3.Distance(transform.position, Player.transform.position) >= 1)
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;


        }
    }*/

}
