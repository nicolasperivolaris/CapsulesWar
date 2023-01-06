using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chromosome : MonoBehaviour
{
    delegate void Action();
    public List<Gene> genes { get;  }
    int totalWeight;

    public Chromosome()
    {
        genes = new List<Gene>();
    }


    // Start is called before the first frame update
    void Start()
    {
        genes.Add(new Gene("Speed", 1, 1, (g)=> { GetComponentInParent<NavMeshAgent>().speed *=g.value/(float)totalWeight; }));
        genes.Add(new Gene("Fly", 0, (g) => { }));
        genes.Add(new Gene("Shoot", 0, (g) => { }));
        genes.Add(new Gene("Jump", 0, (g) => { }));
        genes.Add(new Gene("Multiply", 0, (g) => { }));
        genes.Add(new Gene("Autodestroy", 0, (g) => { }));
    }

    // Update is called once per frame
    void Update()
    {
    }

    static void Mix(Chromosome c1, Chromosome c2)
    {

    }

    public class Gene
    {
        public string name { get; }
        public int value { get; }
        public int minValue { get; }
        internal delegate void Expression(Gene gene);
        Expression expression;

        internal Gene(string name, int value, int minValue,  Expression e)
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
}
