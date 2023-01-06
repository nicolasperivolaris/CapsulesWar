using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e != null)
            Destroy(e.gameObject);

    }*/

    private void OnTriggerEnter(Collider collider)
    {
        Enemy e = collider.gameObject.GetComponent<Enemy>();
        if (e != null)
            Destroy(e.gameObject);

    }
}
