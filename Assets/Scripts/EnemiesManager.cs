using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    private Chromosome seed;
    public GameObject PrefabEnemy;
    public GameObject Player;
    private float elapsedTime = 0f;
    private float interval = 2f;
    // Start is called before the first frame update
    public Saber saber;
    void Start()
    {
        seed = new Chromosome();
        Gene speedGene = new Gene(Gene.SPEED, 1, 1);
        speedGene.FirstEffect = (enemy, gene) => {
            enemy.GetComponent<NavMeshAgent>().speed += gene.value/(float)(seed.totalWeight + 1);
        };
        seed.Add(speedGene);
        speedGene = new Gene(Gene.JUMP, 1, 0);
        speedGene.ContinuousEffect = (enemy, gene) => {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if(rb.velocity.y > 0.1 && Random.Range(0, 200) < 1)
            {
                Vector3 velocity = rb.velocity;
                velocity.y = gene.value/(float)(seed.totalWeight + 1);
                enemy.GetComponent<Rigidbody>().velocity = velocity;
            }
        };
        seed.Add(speedGene);
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
            enemy.transform.parent = this.transform;
            Enemy eComp = enemy.AddComponent<Enemy>();
            eComp.Player = Player;
            seed = Chromosome.Crossover(seed, seed);
            eComp.SetChromosome(seed);
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
        
    }

    public void OnEnemyKilled(Enemy e)
    {
    }
}
