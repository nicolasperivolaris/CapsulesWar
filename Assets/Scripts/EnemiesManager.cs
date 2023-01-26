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
    public Transform PlayerHead;
    public GameObject Laser;
    private float elapsedTime = 0f;
    private float interval = 2f;
    // Start is called before the first frame update
    public Attachable saber;
    private List<Enemy> enemySet;
    private List<Enemy> deadEnemies;
    private float waitTime = float.MaxValue;
    static int WAIT_TIME = 5;
    public float BreedingProportion = 0.3f;
    public int PopulationSize = 30;
    void Start()
    {
        enemySet = new List<Enemy>();
        deadEnemies = new List<Enemy>();

        for(int i = 0; i< PopulationSize; i++)
        {
            GameObject go = Instantiate(PrefabEnemy, Vector3.zero, Quaternion.identity);
            go.SetActive(false);
            go.transform.parent = transform;
            Enemy enemy = go.AddComponent<Enemy>();
            enemy.aim = PlayerHead;
            enemy.Player = Player;
            enemy.laser = Laser;
            enemySet.Add(enemy);
        }
            
    }

    void Update()
    {
        if (waitTime > WAIT_TIME)
        {
            elapsedTime += Time.deltaTime;
            if (saber.IsActivated && elapsedTime >= interval)
            {
                elapsedTime = 0f;
                var e = enemySet.FirstOrDefault(e => !e.gameObject.activeInHierarchy && !deadEnemies.Contains(e));
                if (e != null)
                    DropEnemy(e);
            }

            if (deadEnemies.Count == enemySet.Count)
            {
                UpdateScore();
                waitTime = 0;
                BreedEnemies();
                deadEnemies.Clear();
            }
        } 
        else
        {
            waitTime += Time.deltaTime;
        }
    }

    private void BreedEnemies()
    {
        deadEnemies.Sort((a, b) => (int)(a.Fitness() - b.Fitness()));
        int size = (int)(PopulationSize * BreedingProportion);
        if (size % 2 == 1) size++;
        List<Enemy> parents = deadEnemies.GetRange(0, size);
        enemySet.Clear();
        for (int i = 0; i < PopulationSize; i++)
        {
            int pI = (i / 2);
            GameObject go = Instantiate(PrefabEnemy, Vector3.zero, Quaternion.identity);
            go.SetActive(false);
            go.transform.parent = transform;
            Enemy enemy = go.AddComponent<Enemy>();
            Enemy[] tab = { parents.ElementAt(pI%parents.Count), parents.ElementAt((pI + 1)%parents.Count) };
            enemy.parents = tab;
            enemy.Player = Player;
            enemy.laser = Laser;
            enemySet.Add(enemy);
        }
    }

    private void UpdateScore()
    {

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
            
        Collider collider = closest.GetComponent<Collider>();

        Vector3 position = collider.bounds.center + new Vector3(
        Random.Range(-collider.bounds.size.x / 2, collider.bounds.size.x / 2),
        10,
        Random.Range(-collider.bounds.size.z / 2, collider.bounds.size.z / 2));  

        enemy.transform.position = position;
        enemy.popUpDist = Vector2.Distance(new Vector2(position.x, position.y), new Vector2(Player.transform.position.x, Player.transform.position.y));
        enemy.gameObject.SetActive(true);
    }

    public void OnEnemyKilled(Enemy e)
    {
        deadEnemies.Add(e);
        float tempLT = 0;
        foreach (var dead in deadEnemies)
        {
            tempLT += dead.LifeTime;
        }
        Enemy.AvgLT = tempLT/deadEnemies.Count;
    }
}