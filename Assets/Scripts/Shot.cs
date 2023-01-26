using UnityEngine;
using System.Collections;
using static Enemy;
using Valve.VR.InteractionSystem;
using UnityEngine.AI;
using UnityEditor.UIElements;

public class Shot : MonoBehaviour
{

    public LaserGene shooter;
    Vector3 startPosition;
    const int MAX_LIFT_TIME = 3;
    float lifeTime = 0;
    public int speed = 4;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position = shooter.transform.position + transform.forward * GetComponent<CapsuleCollider>().radius;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if ((lifeTime+=Time.deltaTime) > MAX_LIFT_TIME)
            Destroy(gameObject);
        //transform.position += transform.forward * Time.deltaTime * shooter.value * 10;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, MAX_DISTANCE*2, ~1<<7);
        Vector3 reflexion = Vector3.Reflect(transform.forward, hit.normal);
        transform.forward = reflexion;*/
        if (collision.gameObject.name.Equals("Shield") || collision.gameObject.name.Equals("Blade"))
        {
            Vector3 reflection = Vector3.Reflect(transform.forward, collision.GetContact(0).normal);
            transform.forward = reflection;
            GetComponent<Rigidbody>().velocity = transform.forward * speed;

        }
        else Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHead player = other.GetComponent<PlayerHead>();
        if (player != null)
        {
            player.Touched(this);
            shooter.PlayerTouched++;
        }
        if(other.GetComponent<NavMeshObstacle>() != null)
            Destroy(gameObject);        
    }
}
