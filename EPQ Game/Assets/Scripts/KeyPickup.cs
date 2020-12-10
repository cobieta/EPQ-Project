using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour {

	public AudioClip keyGrab;

	void Start () {

	}

	void OnTriggerEnter (Collider myCollider) {
		if ((myCollider.gameObject.tag == Tags.player) || (myCollider.gameObject.tag == Tags.clone)) {
			PlayerInventory inventory;
			AudioSource.PlayClipAtPoint(keyGrab, transform.position);
			inventory = myCollider.gameObject.GetComponent<PlayerInventory>();
			inventory.hasKey = true;
			Destroy(gameObject);
		}
	} 

}
