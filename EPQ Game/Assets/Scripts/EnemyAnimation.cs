﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour {

	public float deadZone = 5f;

	private AlternativeSight enemySight;
	private NavMeshAgent nav;
	private Animator anim;
	private AnimatorSetup animSetup;
	private CreateNewPlayer newPlayer;

	// Use this for initialization
	void Start () {
		enemySight = GetComponent<AlternativeSight>();
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();

		nav.updateRotation = false;
		animSetup = new AnimatorSetup(anim);
		deadZone *= Mathf.Deg2Rad;
	}
	
	// Update is called once per frame
	void Update () {
		NavAnimSetup();
	}

	void OnAnimatorMove () {
		nav.velocity = anim.deltaPosition / Time.deltaTime;
		transform.rotation = anim.rootRotation;
	}

	void NavAnimSetup () {
		float speed;
		float angle;

		if (enemySight.playerInSight) {
			speed = 0f;
			Vector3 playerOrClonePosition = enemySight.personalLastSighting;
			Vector3 enemyToPlayerVec = playerOrClonePosition - transform.position;
			angle = FindAngle(transform.forward, enemyToPlayerVec, transform.up);
		} else {
			speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
			angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);
			if (Mathf.Abs(angle) < deadZone) {
				transform.LookAt(transform.position + nav.desiredVelocity);
				angle = 0f;
			}
		}
		animSetup.Setup(speed, angle);
	}

	float FindAngle (Vector3 fromVector, Vector3 toVector, Vector3 upVector) {
		if (toVector == Vector3.zero) {
			return 0f;
		}
		float angle = Vector3.Angle(fromVector, toVector);
		Vector3 normal = Vector3.Cross(fromVector, toVector);
		angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
		angle *= Mathf.Deg2Rad;
		return angle;
	}
}
