using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerCollider : MonoBehaviour
{
    public CapsuleCollider Collider;
    public Transform ToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Collider.center = new Vector3(ToFollow.localPosition.x, Collider.center.y, ToFollow.localPosition.z);
    }
}
