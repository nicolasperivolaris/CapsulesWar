using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    public GameObject PrefabEnemy;
    public GameObject Player;
    private float elapsedTime = 0f;
    public float interval = 2f;
    // Start is called before the first frame update
    public Saber saber;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (saber.IsActivated && elapsedTime >= interval)
        {
            elapsedTime = 0f;
            foreach (var enemy in GetComponentsInChildren<Enemy>())
            {
                if(!enemy.enabled) enemy.enabled = true;
            }
            DropEnemy();
        }
    }

    private void DropEnemy()
    {
        MeshCollider collider = GameObject.Find("GamePlane").GetComponent<MeshCollider>();

        Vector3 position = collider.bounds.center + new Vector3(
        Random.Range(-collider.bounds.size.x / 2, collider.bounds.size.x / 2),
        10,
        Random.Range(-collider.bounds.size.z / 2, collider.bounds.size.z / 2)
);

        GameObject enemy = Instantiate(PrefabEnemy, position, Quaternion.identity);
        enemy.transform.parent = this.transform;
        Enemy e = enemy.GetComponent<Enemy>();
        e.enabled = true;
        e.Player = Player;
        
    }

    public void OnEnemyKilled(Enemy e)
    {
        DropEnemy();
    }
}
