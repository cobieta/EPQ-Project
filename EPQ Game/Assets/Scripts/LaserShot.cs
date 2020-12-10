using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShot : MonoBehaviour {

	public float shotLifeTime = 15f;
	public float shotSpeed = 1f;
	public int shotFromGuardNumber = 1;

	private Rigidbody rBody;

	// Use this for initialization
	void Awake () {
		rBody = GetComponent<Rigidbody>();
		Invoke("destroyProjectile", shotLifeTime);
	}

	public void ShotTarget () {
		//Vector3 SVelo = new Vector3(0f,0f,shotSpeed);
		//Debug.Log(SVelo);
		Vector3 SVelo = transform.forward * shotSpeed;
		rBody.velocity = SVelo;
		//Debug.Log(rBody.velocity);
	}

	void destroyProjectile () {
		DestroyObject(gameObject);
	}

	void OnColliderEnter (Collider myCollider) {
		if (myCollider.gameObject.tag == Tags.enemy) {
			EnemyShooting enemShoot = myCollider.gameObject.GetComponent<EnemyShooting>();
			if (enemShoot.guardNumber != shotFromGuardNumber) {
				destroyProjectile();
			}
		} else {
			destroyProjectile();
		}
	}
}
