using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour {

	public bool requireKey = false;
	public AudioClip doorSwishClip, accessDeniedClip;

	private Animator anim;
	private int count;
	private AudioSource myAudio;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		myAudio = GetComponent<AudioSource>();
	}

	void Update () {
		anim.SetBool("OpenBool", count > 0);
		if (anim.IsInTransition(0) && !myAudio.isPlaying) {
			myAudio.clip = doorSwishClip;
			myAudio.Play();
		}
	}

	void OnTriggerEnter (Collider myCollider) {
		if ((myCollider.gameObject.tag == Tags.player) || (myCollider.gameObject.tag == Tags.clone)) {
			if (requireKey) {
				PlayerInventory inventory;
				inventory = myCollider.gameObject.GetComponent<PlayerInventory>();
				if (inventory.hasKey) {
					count ++;
				} else {
					myAudio.clip = accessDeniedClip;
					myAudio.Play();
				}
			} else {
				count ++;
			}
		} else if (myCollider.gameObject.tag == Tags.enemy) {
			if (myCollider is CapsuleCollider) {
				count ++;
			}
		}
	}

	void OnTriggerExit (Collider myCollider) {
		if (myCollider.gameObject.tag == Tags.player || myCollider.gameObject.tag == Tags.clone || (myCollider.gameObject.tag == Tags.enemy && myCollider is CapsuleCollider)) {
			count = Mathf.Max(0, count-1);
		}
	}
}
