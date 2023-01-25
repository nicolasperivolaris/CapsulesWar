using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Teleporter : MonoBehaviour
{
    private float elapsedTime = 0f;
    public Teleporter destination;
    bool activated = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated && elapsedTime<5)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 5)
            {
                GetComponent<MeshRenderer>().material.color = Color.white;
                activated = true;
                elapsedTime = 0;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if(player != null)
            if(activated)
            {
                activated = false;
                destination.activated = false;
                Rigidbody r = player.GetComponent<Rigidbody>();
                r.useGravity = true;
                r.isKinematic = false;
                r.constraints &= ~RigidbodyConstraints.FreezePositionY;
                player.transform.position = destination.transform.position + Vector3.up*5;
                GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else
            {
                Rigidbody r = player.GetComponent<Rigidbody>();
                r.useGravity = false;
                r.isKinematic = true;
                r.constraints |= RigidbodyConstraints.FreezePositionY;
            }
    }
}
