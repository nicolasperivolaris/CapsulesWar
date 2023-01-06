using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome : MonoBehaviour
{
    delegate void Action();
    List<Gene> genes;


    // Start is called before the first frame update
    void Start()
    {
        genes = new List<Gene>();
        genes.Add(new Gene("Speed", 1, 1, ()=> {}));
        genes.Add(new Gene("Fly", 1, 1, () => { }));
        genes.Add(new Gene("Shoot", 1, 1, () => { }));
        genes.Add(new Gene("Jump", 1, 1, () => { }));
        genes.Add(new Gene("Multiply", 1, 1, () => { }));
        genes.Add(new Gene("Autodestroy", 1, 1, () => { }));

    }

    // Update is called once per frame
    void Update()
    {
    }

    static void Mix(Chromosome c1, Chromosome c2)
    {

    }

    class Gene
    {
        private string name;
        private int value;
        private int minValue;
        private Action expression;

        internal Gene(string name, int value, int minValue, Action expression)
        {
            this.name = name;
            this.value = value;
            this.minValue = minValue;
            this.expression = expression;
        }

        internal void Express()
        {
            expression();
        }

        
    }
}
