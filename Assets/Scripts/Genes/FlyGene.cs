using UnityEngine.AI;
using UnityEngine;

public partial class Enemy
{
    public class FlyGene : Gene
    {
        bool flying = false;
        float flyingTime = 0;
        public FlyGene() : base(Gene.FLY, 0, 0)
        {
        }

        void Update()
        {
            if (Random.Range(0, (int)(50 * (6 - 1 / (value + .0001f)))) == 0)
            {
                flying = true;
                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<NavMeshObstacle>().enabled = true;
            }
            if (flying)
            {            
                if (flyingTime >= value)
                {
                    flying = false;
                } 

                GetComponent<Rigidbody>().velocity = GetComponent<NavMeshAgent>().speed * transform.forward + Vector3.up ;
                flyingTime += Time.deltaTime;
            }
        }

        public override int getFitBonus()
        {
            return 0;
        }
    }
}