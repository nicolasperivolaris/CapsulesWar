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
    // Start is called before the first frame update
    public Saber saber;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (saber.IsActivated && transform.childCount == 0)
        {
            foreach (var enemy in GetComponentsInChildren<Enemy>())
            {
                enemy.enabled = true;
            }
            DropEnemy();
        }
    }

    private void DropEnemy()
    {
        GameObject gamePlane = GameObject.Find("GamePlane");
        Vector3 dimensions = gamePlane.transform.lossyScale;
        Vector3 position = gamePlane.transform.position;

        float minX = position.x - dimensions.x / 2;
        float maxX = position.x + dimensions.x / 2;
        float minZ = position.z - dimensions.z / 2;
        float maxZ = position.z + dimensions.z / 2;

        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);

        GameObject enemy = Instantiate(PrefabEnemy, new Vector3(x, 1, z), Quaternion.identity);
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
