using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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
    private List<Enemy> enemySet;
    private List<Enemy> deadEnemies;
    private const int DEAD_QUEUE_SIZE = 10;
    void Start()
    {
        enemySet = new List<Enemy>();
        deadEnemies = new List<Enemy>();

        for(int i = 0; i<DEAD_QUEUE_SIZE*10; i++)
        {
            GameObject go = Instantiate(PrefabEnemy, Vector3.zero, Quaternion.identity);
            go.SetActive(false);
            go.transform.parent = transform;
            Enemy enemy = go.AddComponent<Enemy>();
            enemy.Player = Player;
            enemy.laser = Laser;
            enemySet.Add(enemy);
        }
            
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (saber.IsActivated && elapsedTime >= interval)
        {
            elapsedTime = 0f;
            DropEnemy(enemySet.First(e => !e.gameObject.activeInHierarchy && !deadEnemies.Contains(e)));
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
        enemy.gameObject.SetActive(true);
    }

    public void OnEnemyKilled(Enemy e)
    {
        deadEnemies.Add(e);
        float tempLT = 0;
        foreach (var dead in deadEnemies)
        {
            tempLT += dead.lifeTime;
        }
        Enemy.AvgLT = tempLT/deadEnemies.Count;
    }
}