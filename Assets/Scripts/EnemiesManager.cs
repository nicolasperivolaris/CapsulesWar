using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    public GameObject PrefabEnemy;
    public GameObject Player;
    private float elapsedTime = 0f;
    private float interval = 2f;
    // Start is called before the first frame update
    public Saber saber;
    private SortedList<float, Enemy> deadEnemies = new SortedList<float, Enemy>();
    private const int DEAD_QUEUE_SIZE = 10;
    private int enemyCount = 0;
    void Start()
    {
        deadEnemies.Capacity = DEAD_QUEUE_SIZE;
        Chromosome c = gameObject.AddComponent<Chromosome>();
        c.enabled = false;
        c.Add<Enemy.SpeedGene>(1).enabled = false;

        /*
        seed = new Chromosome();
        AbstractGene speedGene = new Enemy.SpeedGene();
        seed.Add(speedGene);
        speedGene = new Gene(Gene.JUMP, 1, 0);
        speedGene.ContinuousEffect = (enemy, gene) => {
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            //enemy.GetComponent<MeshRenderer>().material.color = Color.green;
            if (Random.Range(0, 100) < 1)
            {
                enemy.Jump();
                //agent.velocity.y = gene.value/(float)(seed.totalWeight + 1);
            }
        };
        seed.Add(speedGene);*/
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (saber.IsActivated && elapsedTime >= interval)
        {
            elapsedTime = 0f;
            foreach (var e in GetComponentsInChildren<Enemy>())
            {
                if(!e.enabled) e.enabled = true;
            }

            GameObject enemy = Instantiate(PrefabEnemy, Vector3.zero, Quaternion.identity);
            enemy.transform.parent = transform;
            Enemy eComp = enemy.AddComponent<Enemy>();
            eComp.Player = Player;
            Chromosome c = eComp.AddComponent<Chromosome>();
            c.Copy(GetComponent<Chromosome>());
            DropEnemy(enemy);
        }
    }

    private void DropEnemy(GameObject enemy)
    {
        MeshCollider collider = GameObject.Find("GamePlane").GetComponent<MeshCollider>();

        Vector3 position = collider.bounds.center + new Vector3(
        Random.Range(-collider.bounds.size.x / 2, collider.bounds.size.x / 2),
        10,
        Random.Range(-collider.bounds.size.z / 2, collider.bounds.size.z / 2));  

        enemy.transform.position = position;
        enemyCount++;
    }

    public void OnEnemyKilled(Enemy e)
    {
        if(!deadEnemies.ContainsKey(e.Fitness()))
            deadEnemies.Add(e.Fitness(), e);
        deadEnemies.TrimExcess();
        long tempLT = 0;
        foreach (var dead in deadEnemies)
        {
            tempLT += dead.Value.getLifeTime();
        }
        Enemy.AvgLT = tempLT/deadEnemies.Count;
    }
}