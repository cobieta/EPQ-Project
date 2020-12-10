using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlternativeSight : MonoBehaviour {

	public float viewRadius = 6f;
	[Range(0,360)]
	public float viewAngle = 110f;
	public LayerMask targetMask, obstacleMask;
	public List<Transform> visibleTargets = new List<Transform>();

	public bool playerInSight;
	public AudioClip alarmClip;
	public Vector3 personalLastSighting;
	public GameObject personalLastSightedObject;

	private NavMeshAgent nav;
	private Animator anim;
	private Vector3 previousSighting;
	private HashIDs hash;
	private LastPlayerSighting lastPlayerSighting;
	private bool alarmHasPlayed = false;

	void Start () {
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		personalLastSighting = lastPlayerSighting.resetPosition;
		previousSighting = personalLastSighting;
		StartCoroutine("FindTargetsWithDelay", 0.2f);
	}

	// Update is called once per frame
	void Update () {
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

	IEnumerator FindTargetsWithDelay (float delay) {
		while (true) {
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	void FindVisibleTargets () {
		//Debug.Log("HI");
		visibleTargets.Clear();
		playerInSight = false;
		//alarmHasPlayed = false;
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			if (targetsInViewRadius[i].tag == Tags.player || targetsInViewRadius[i].tag == Tags.clone) {
				Transform target = targetsInViewRadius[i].transform;
				Debug.Log(target);
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2) {
					float distToTarget = Vector3.Distance(transform.position, target.position);
					if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) {
						visibleTargets.Add(target);
						PlayerHealth playerHealth = target.gameObject.GetComponent<PlayerHealth>();
						Debug.Log(playerHealth);
						if (playerHealth.health > 0f) {
							// sees target.
							visibleTargets.Add(target);
							playerInSight = true;
							if (!alarmHasPlayed) {
								AudioSource.PlayClipAtPoint(alarmClip, transform.position);
								alarmHasPlayed = true;
							}
							Debug.Log(target.position);
							personalLastSighting = target.position;
							personalLastSightedObject = target.gameObject;
						}
					}
				}
			} 
		}
	}

	public Vector3 DirFromAngle (float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		float angle = angleInDegrees * Mathf.Deg2Rad;
		Vector3 direction = new Vector3(Mathf.Sin(angle),0f, Mathf.Cos(angle));
		return direction;
	}
}
