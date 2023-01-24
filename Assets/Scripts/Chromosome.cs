using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Chromosome:MonoBehaviour
{
    public int totalWeight { get; private set; }
    private Dictionary<string, Gene> genes = new Dictionary<string, Gene>();

    public void Crossover(Chromosome c1, Chromosome c2)
    {
        for(int i=0; i<genes.Count; i++)
        {
            Gene toAdd = null;
            if (i < genes.Count / 2)
                toAdd = c1.genes.ElementAt(i).Value;
            else
                toAdd = c2.genes.ElementAt(i).Value;
            Add(toAdd);
        }
        if(Random.Range(0,10) == 0)
            Mutate() ;
    }

    public void Init()
    {
        Add<Enemy.SpeedGene>();
        Add<Enemy.JumpGene>();
        Add<Enemy.LaserGene>();
        Add<Enemy.FlyGene>();
    }

    private void Add(Gene g)
    {
        System.Type[] typeArgs = { g.GetType() };
        MethodInfo addMethod = GetType().GetMethod("Add", new System.Type[] { typeof(int) }).MakeGenericMethod(typeArgs);
        addMethod.Invoke(this, new object[] { g.value });
    }

    public void Mutate()
    {
        int type = Random.Range(0, 3);
        switch (type)
        {
            case 0:
                genes.ElementAt(Random.Range(0, genes.Count)).Value.value++;
                break;
            case 1:
                Gene toChange = genes.ElementAt(Random.Range(0, genes.Count)).Value;
                if(toChange.value != toChange.minValue)
                    toChange.value--;
                break;
            case 2:
                Gene a = genes.ElementAt(Random.Range(0, genes.Count)).Value;
                Gene b = genes.ElementAt(Random.Range(0, genes.Count)).Value;
                int temp = b.value;
                b.value = a.value>b.minValue ? a.value : b.minValue;
                a.value = temp>a.minValue ? temp : a.minValue;
                break;
        }
    }

    public void Copy(Chromosome c)
    {
        foreach(Gene gene in c.genes.Values)
        {
            Add(gene);
        }
    }

    public bool Remove(Gene gene)
    {
        if (genes.ContainsKey(gene.type))
        {
            totalWeight -= gene.value;
            genes.Remove(gene.type);
            Destroy(gene);
            return true;
        }
        return false;
    }

    public T Add<T>(int value) where T : Gene
    { 
        T newGene = gameObject.AddComponent<T>();
        newGene.value = value;
        genes.Add(newGene.type, newGene);
        totalWeight += value;
        return newGene;
    }

    public T Add<T>() where T : Gene
    {
        T newGene = gameObject.AddComponent<T>();
        genes.Add(newGene.type, newGene);
        totalWeight += newGene.value;
        return newGene;
    }

    public Dictionary<string, Gene> GetGenes()
    {
        return genes;
    }

    internal int getGenesFitBonus()
    {
        int result = 0;
        foreach(var gene in genes.Values)
        {
            result += gene.getFitBonus();
        }

        return result;
    }
}
