using System;
using UnityEngine.AI;

public partial class Enemy
{
    public class SpeedGene:Gene
    {
        public SpeedGene() : base(SPEED, 1, 1)
        {}

        public override int getFitBonus()
        {
            if (GetComponent<Enemy>().distanceToPlayer < 3) return 50;
            else return 0;
        }

        public void Update()
        {
            GetComponentInParent<NavMeshAgent>().speed = (float)(Math.Sqrt(proportionnalValue()));
        }
    }
}