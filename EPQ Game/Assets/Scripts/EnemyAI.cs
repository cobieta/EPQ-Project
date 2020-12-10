using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

	public float patrolSpeed = 2f;
	public float chaseSpeed = 5f;
	public float chaseWaitTime = 5f;
	public float patrolWaitTime = 1f;
	public Transform[] patrolWayPoints;

	private AlternativeSight enemySight;
	private NavMeshAgent nav;
	private CreateNewPlayer newPlayer;
	private PlayerHealth playerHealth;
	private LastPlayerSighting lastPlayerSighting;
	private float chaseTimer;
	private float patrolTimer;
	private int wayPointIndex;

	// Use this for initialization
	void Start () {
		enemySight = GetComponent<AlternativeSight>();
		nav = GetComponent<NavMeshAgent>();
		newPlayer = GameObject.FindGameObjectWithTag(Tags.cloneController).GetComponent<CreateNewPlayer>();
		playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();

	}
	
	// Update is called once per frame
	void Update () {
		if (newPlayer.playerRespawned) {
			playerHealth = newPlayer.currentPlayer.GetComponent<PlayerHealth>();
		}
		//Debug.Log(enemySight.personalLastSighting);
		if (enemySight.playerInSight && (playerHealth.health > 0f)) {
			nav.isStopped = true;
			Debug.Log("I am shooting.");
		} else if ((enemySight.personalLastSighting != lastPlayerSighting.resetPosition) && (playerHealth.health > 0f)) {
			Chasing();
			Debug.Log("I am chasing.");
		} else {
			Patrolling();
			Debug.Log("I am patrolling.");
		}
	}

	void Chasing () {
		nav.isStopped = false;
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
		//Debug.Log(sightingDeltaPos.sqrMagnitude);
		if (sightingDeltaPos.sqrMagnitude > 4f) {
			nav.SetDestination(enemySight.personalLastSighting);
			//Debug.Log(nav.destination);
			//Debug.Log(enemySight.personalLastSighting);
		}
		nav.speed = chaseSpeed;
		//Debug.Log(nav.remainingDistance);
		if (nav.remainingDistance <= nav.stoppingDistance) {
			chaseTimer += Time.deltaTime;
			//Debug.Log("Hi 2");
			if (chaseTimer > chaseWaitTime) {
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
				enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
				chaseTimer = 0f;
				//Debug.Log("Hi 3");
			}
		} else {
			chaseTimer = 0f;
			//Debug.Log("Hi 4");
		}
	}

	void Patrolling () {
		nav.isStopped = false;
		nav.speed = patrolSpeed;
		if ((nav.destination == lastPlayerSighting.resetPosition) || (nav.remainingDistance < nav.stoppingDistance)) {
			patrolTimer += Time.deltaTime;
			if (patrolTimer >= patrolWaitTime) {
				if (wayPointIndex == patrolWayPoints.Length -1 ) {
					wayPointIndex = 0;
				} else {
					wayPointIndex++;
				}
				patrolTimer = 0f;
			}
		} else {
			patrolTimer = 0f;
		}
		nav.destination = patrolWayPoints[wayPointIndex].position;
	}
}
