using UnityEngine.AI;
using UnityEngine;

public partial class Enemy
{
    public class JumpGene : Gene
    {
        public JumpGene() : base(Gene.JUMP, 0, 0)
        {
        }

        void Update()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            
            if (agent.enabled && Random.Range(0, 300) == 0)
            {//jump
                agent.enabled = false;
                GetComponent<MeshRenderer>().material.color = Color.green;
                GetComponent<NavMeshObstacle>().enabled = true;
                agent.enabled = false;
                GetComponent<Rigidbody>().isKinematic = false;
                Vector3 rbV = GetComponent<Rigidbody>().velocity;
                Vector3 nmaV = agent.velocity;
                GetComponent<Rigidbody>().velocity = agent.transform.forward*5 + new Vector3(0,5, 0);
            }
        }

        public override int getFitBonus()
        {
            return 0;
        }
    }
}