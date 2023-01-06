using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Chromosome : Dictionary<string, Gene>
{
    delegate void Action();

    private const string SPEED = "Speed";
    private const string FLY = "Fly";
    private const string SHOOT = "Shoot";
    private const string JUMP = "Jump";
    private const string MULTIPLY = "Multiply";
    private const string AUTIDESTROY = "Autodestroy";
    internal int totalWeight;

    public Chromosome() : base()
    { 
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    static Chromosome Mix(Chromosome c1, Chromosome c2)
    {
        Chromosome newChro = new Chromosome();
        foreach (var c in c1)
        {
            if (c1.ContainsKey(c.Key)) {
                Gene g = Random.Range(0, 2) == 0 ? c.Value : c2[c.Key];
                newChro.Add(g.name, g);
            }
            else
            {
                if (Random.Range(0, 2) == 0) newChro.Add(c.Key, c.Value);
            }
        }

        foreach (var c in c2)
        {
            if(!newChro.ContainsKey(c.Key) && Random.Range(0, 2) == 0) 
                newChro.Add(c.Key, c.Value);
        }

        return newChro;
    }

    

    public void Add(Gene gene)
    {
        if (ContainsKey(gene.name))
        {
            totalWeight -= this[gene.name].value;
            Remove(gene.name);
        }
        Add(gene.name, gene);
        totalWeight += gene.value;
    }
}
public class Gene
{
    public string name { get; }
    public int value { get; }
    public int minValue { get; }
    internal delegate void Expression(Gene gene);
    Expression expression;

    internal Gene(string name, int value, int minValue, Expression e)
    {
        this.name = name;
        this.value = value;
        this.minValue = minValue;
        this.expression = e;
    }

    internal Gene(string name, int value, Expression e) : this(name, value, 0, e) { }

    internal void Express()
    {
        expression(this);
    }


}