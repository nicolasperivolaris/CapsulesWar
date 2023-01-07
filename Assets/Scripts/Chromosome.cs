using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Chromosome : Dictionary<string, Gene>
{
    delegate void Action();
    internal int totalWeight;

    public Chromosome() : base()
    { 
        
    }

    public static Chromosome Crossover(Chromosome c1, Chromosome c2)
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

        newChro.Mutate() ;

        return newChro;
    }

    private void Mutate()
    {
        foreach (var g in this)
        {
            if (Random.Range(0, 2) == 0)
            {
                g.Value.value++;
            }
            if (Random.Range(0, 2) == 0)
            {
                if (g.Value.value > g.Value.minValue) g.Value.value--;
            }
            if (Random.Range(0, 10) == 0)
            {
                g.Value.value = g.Value.minValue;
            }
        }
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
    public const string SPEED = "Speed";
    public const string FLY = "Fly";
    public const string SHOOT = "Shoot";
    public const string JUMP = "Jump";
    public const string MULTIPLY = "Multiply";
    public const string AUTIDESTROY = "Autodestroy";

    public string name { get; }
    public int value { get; set; }
    public int minValue { get; }
    public delegate void Expression(Enemy e, Gene g);
    public Expression expression { get; set; }

    public Gene(string name, int value, int minValue)
    {
        this.name = name;
        this.value = value;
        this.minValue = minValue;
    }

    public Gene(string name, int value) : this(name, value, 0) { }

    public void Express(Enemy e)
    {
        expression(e, this);
    }


}