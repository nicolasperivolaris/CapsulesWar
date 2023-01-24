using UnityEngine;
using System.Collections;
using static Enemy;
using Valve.VR.InteractionSystem;

public class ShotBehavior : MonoBehaviour {

	public LaserGene shooter;
	Vector3 startPosition;
	const int MAX_DISTANCE = 20;

	// Use this for initialization
	void Start () {
		startPosition = transform.position = shooter.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(startPosition, transform.position) > MAX_DISTANCE)
			Destroy(gameObject);
		transform.position += transform.forward * 0.5f * Time.deltaTime * shooter.value * 10;
	}

    private void OnTriggerEnter(Collider other)
    {
		PlayerBody player = other.GetComponent<PlayerBody>();
		if (player != null)
		{
			//player.Touched(this);
			shooter.PlayerTouched++;
		}
		Destroy(gameObject);
    }
}
