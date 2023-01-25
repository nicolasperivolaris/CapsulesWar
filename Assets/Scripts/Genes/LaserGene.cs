using UnityEngine.AI;
using UnityEngine;

public partial class Enemy
{
    public class LaserGene : Gene
    {
        public int PlayerTouched { get; set; }
        public LaserGene() : base(Gene.LASER, 8, 0)
        {}

        void Update()
        {
            if(Random.Range(0, (int)(50*(6 / (value + .0001f)))) == 0)
            {
                GameObject laser = Instantiate(GetComponentInParent<Enemy>().laser, transform.position, Quaternion.identity);
                laser.GetComponent<Shot>().shooter = this;
                laser.transform.parent = transform.parent;
                laser.transform.forward = transform.forward;
            }
        }

        public override int getFitBonus()
        {
            return PlayerTouched * 50;
        }
    }
}
