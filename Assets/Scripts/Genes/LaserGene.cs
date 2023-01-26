using UnityEngine.AI;
using UnityEngine;

public partial class Enemy
{
    public class LaserGene : Gene
    {
        public int PlayerTouched { get; set; }
        public LaserGene() : base(Gene.LASER, 1, 0)
        {}

        void Update()
        {
            if(Random.Range(0, (int)(150*(1 /(2*proportionnalValue())))) == 0)
            {
                GameObject laser = Instantiate(GetComponentInParent<Enemy>().laser, transform.position, Quaternion.identity);
                laser.GetComponent<Shot>().shooter = this;
                laser.transform.parent = transform.parent;
                laser.transform.LookAt(GetComponentInParent<Enemy>().aim);
            }
        }

        public override int getFitBonus()
        {
            return PlayerTouched * 50;
        }
    }
}
