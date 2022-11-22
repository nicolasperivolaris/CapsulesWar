using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Saber saber;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (saber.IsActivated)
        {
            foreach (var enemy in GetComponentsInChildren<Enemy>())
            {
                enemy.enabled = true;
            }
            enabled = false;
        }
    }
}
