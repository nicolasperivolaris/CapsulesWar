using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Gene : MonoBehaviour
{
    public const string SPEED = "Speed";
    public const string FLY = "Fly";
    public const string LASER = "Laser";
    public const string JUMP = "Jump";
    public const string MULTIPLY = "Multiply";
    public const string AUTODESTROY = "Autodestroy";
    public string type { get; }
    public int value { get; set; }
    public int minValue { get; }

    public Gene(string type, int value, int minValue):base()
    { 
        this.type = type;
        this.value = value;
        this.minValue = minValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float proportionnalValue()
    {
        return (value / (float)GetComponent<Chromosome>().totalWeight) + 0.00001f;
    }

    public abstract int getFitBonus();
}
