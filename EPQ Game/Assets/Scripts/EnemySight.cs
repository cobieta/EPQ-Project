using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour {

	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public AudioClip alarmClip;
	public Vector3 personalLastSighting;
	public bool hearingEnabled = false;
	public bool hiveMind = false;
	public GameObject personalLastSightedObject;

	private NavMeshAgent nav;
	private SphereCollider colliderSphere;
	private Animator anim;
	private Vector3 previousSighting;
	private HashIDs hash;
	private LastPlayerSighting lastPlayerSighting;
	private bool alarmHasPlayed = false;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent>();
		colliderSphere = GetComponent<SphereCollider>();
		anim = GetComponent<Animator>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		personalLastSighting = lastPlayerSighting.resetPosition;
		previousSighting = personalLastSighting;
	}
	
	// Update is called once per frame
	void Update () {
		//if (newPlayer.playerRespawned) {
			//playerInSight = false;
			//personalLastSighting = lastPlayerSighting.resetPosition;
			//player = newPlayer.currentPlayer;
			//playerHealth = player.GetComponent<PlayerHealth>();
		//}
		if (lastPlayerSighting.position != previousSighting) {
			personalLastSighting = lastPlayerSighting.position;
		}
		previousSighting = lastPlayerSighting.position;

		if (playerInSight == true) {
			anim.SetBool(hash.playerInSightBool, true);
		} else {
			anim.SetBool(hash.playerInSightBool, false);
			personalLastSighting = lastPlayerSighting.resetPosition;
		}
	}

	void OnTriggerStay (Collider myCollider) {
		if (myCollider.gameObject.tag == Tags.player || myCollider.gameObject.tag == Tags.clone) {
			playerInSight = false;
			Vector3 direction = myCollider.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			Debug.Log(name + "Player is in collider");
			if (angle < (fieldOfViewAngle * 0.5f)) {
				RaycastHit hit;
				Debug.DrawRay(transform.position+transform.up, direction.normalized, Color.white, 3f);
				if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, colliderSphere.radius)) {
					if (hit.collider.gameObject.tag == Tags.player || hit.collider.gameObject.tag == Tags.clone) {
						PlayerHealth playerHealth = hit.collider.gameObject.GetComponent<PlayerHealth>();
						if (playerHealth.health > 0f) {
							playerInSight = true;
							if (!alarmHasPlayed) {
								AudioSource.PlayClipAtPoint(alarmClip, transform.position);
								alarmHasPlayed = true;
							}
							if (hiveMind) {
								lastPlayerSighting.position = myCollider.gameObject.transform.position;
							} else {
								personalLastSighting = myCollider.gameObject.transform.position;
								personalLastSightedObject = myCollider.gameObject;
							}
						}
					}
				}
			}
			if (hearingEnabled == true) {
				HearPlayer(myCollider.gameObject);
			}
		}
	}

	void HearPlayer (GameObject playerOrClone) {
		Animator playerAnim;
		playerAnim = playerOrClone.GetComponent<Animator>();
		int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).fullPathHash;
		//int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).fullPathHash;
		if (playerLayerZeroStateHash == hash.groundedState) {
			if (CalculatePathLength(playerOrClone.transform.position) <= colliderSphere.radius) {
				personalLastSighting = playerOrClone.transform.position;
			}
		}
	}

	float CalculatePathLength (Vector3 targetPosition) {
		NavMeshPath path = new NavMeshPath();
		if (nav.enabled) {
			nav.CalculatePath(targetPosition, path);
		}
		Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
		allWayPoints[0] = transform.position;
		allWayPoints[allWayPoints.Length-1] = targetPosition;
		for (int i = 0; i < path.corners.Length; i++) {
			allWayPoints[i+1] = path.corners[i];
		}
		float pathLength = 0f;
		for (int j = 0; j < allWayPoints.Length-1; j++) {
			pathLength += Vector3.Distance(allWayPoints[j], allWayPoints[j+1]);
		}
		return pathLength;
	}

	void OnTriggerExit (Collider myCollider) {
		if (myCollider.gameObject.tag == Tags.player || myCollider.gameObject.tag == Tags.clone) {
			playerInSight = false;
			alarmHasPlayed = false;
		}
	}
}
