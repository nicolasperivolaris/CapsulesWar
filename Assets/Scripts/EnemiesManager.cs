using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    public GameObject PrefabEnemy;
    public GameObject Player;
    public GameObject Laser;
    private float elapsedTime = 0f;
    private float interval = 2f;
    // Start is called before the first frame update
    public Saber saber;
    private HashSet<Enemy> deadEnemies = new HashSet<Enemy>();
    private const int DEAD_QUEUE_SIZE = 10;
    void Start()
    {
        Chromosome c = gameObject.AddComponent<Chromosome>();
        c.enabled = false;
        c.Add<Enemy.SpeedGene>().enabled = false;
        c.Add<Enemy.JumpGene>().enabled = false;
        c.Add<Enemy.LaserGene>().enabled=false;
        c.Add<Enemy.FlyGene>().enabled = false;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (saber.IsActivated && elapsedTime >= interval)
        {
            elapsedTime = 0f;

            GameObject go = Instantiate(PrefabEnemy, Vector3.zero, Quaternion.identity);
            go.transform.parent = transform;
            Enemy enemy = go.AddComponent<Enemy>();
            enemy.Player = Player;
            enemy.laser = Laser;
            Chromosome c = enemy.AddComponent<Chromosome>();
            c.Copy(GetComponent<Chromosome>());
            DropEnemy(enemy);
        }
    }

    private void DropEnemy(Enemy enemy)
    {
        GameObject[] planes = GameObject.FindGameObjectsWithTag("GamePlane");
        GameObject closest = planes[0];
        foreach (var plane in planes)
        {
            if(Vector3.Distance(plane.transform.position, Player.transform.position) < Vector3.Distance(closest.transform.position, Player.transform.position))
            {
                closest = plane;
            }
        }
            
        MeshCollider collider = closest.GetComponent<MeshCollider>();

        Vector3 position = collider.bounds.center + new Vector3(
        Random.Range(-collider.bounds.size.x / 2, collider.bounds.size.x / 2),
        10,
        Random.Range(-collider.bounds.size.z / 2, collider.bounds.size.z / 2));  

        enemy.transform.position = position;
    }

    public void OnEnemyKilled(Enemy e)
    {
        deadEnemies.Add(e);
        long tempLT = 0;
        foreach (var dead in deadEnemies)
        {
            tempLT += dead.getLifeTime();
        }
        Enemy.AvgLT = tempLT/deadEnemies.Count;
    }
}