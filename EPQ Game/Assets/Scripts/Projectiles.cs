using UnityEngine;
using System.Collections;

public class Projectiles : MonoBehaviour {

	public float speed, damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		//Attackers attacker = collider.gameObject.GetComponent<Attackers> ();
		//Health health = collider.gameObject.GetComponent<Health> ();

		//if (attacker && health) {
			//health.DealDamage(damage);
			//Destroy(gameObject);
		//}
	}
}
