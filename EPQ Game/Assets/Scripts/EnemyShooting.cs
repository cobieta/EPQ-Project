using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

	public float shotDamage = 100f;
	public AudioClip shotClip, chargeClip;
	public int guardNumber = 1;
	public GameObject shotPrefab;

	private ParticleSystem particles;
	private Animator anim;
	private LineRenderer laserShotLine;
	private Light laserShotLight;
	private SphereCollider mySphereCol;
	private Vector3 playerPosition;
	private PlayerHealth playerHealth;
	private HashIDs hash;
	private CreateNewPlayer newPlayer;
	private bool shooting = false;
	private AlternativeSight enemySight;
	private PlayerMovementRecorder movesRecorder;
	private Transform gunTransform;

	// Use this for initialization
	void Start () {
		newPlayer = GameObject.FindGameObjectWithTag(Tags.cloneController).GetComponent<CreateNewPlayer>();
		particles = GetComponentInChildren<ParticleSystem>();
		anim = GetComponent<Animator>();
		laserShotLine = GetComponentInChildren<LineRenderer>();
		laserShotLight = laserShotLine.gameObject.GetComponent<Light>();
		mySphereCol = GetComponent<SphereCollider>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		movesRecorder = GameObject.FindObjectOfType<PlayerMovementRecorder>();
		enemySight = GetComponent<AlternativeSight>();
		gunTransform = laserShotLine.gameObject.transform;

		laserShotLine.enabled = false;
		laserShotLight.intensity = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		//if (newPlayer.playerRespawned) {
			//playerTransform = newPlayer.currentPlayer.transform;
		//}
		float laserLightIntensity = laserShotLight.intensity;
		//Debug.Log(shooting);
		//Debug.Log(laserLightIntensity);
		if ((shooting == false) && (laserLightIntensity >= 3f)) {
			//Debug.Log("HI");
			//GetTarget();
			//Shoot();
			ShootShot();
		} 
		if (laserLightIntensity < 3f) {
			shooting = false;
			//laserShotLine.enabled = false;
		}
	}

	void GetTarget () {
		playerPosition = enemySight.personalLastSighting;
		playerHealth = enemySight.personalLastSightedObject.GetComponent<PlayerHealth>();
	}

	void ShootShot () {
		playerPosition = enemySight.personalLastSighting;
		shooting = true;
		movesRecorder.SetKillerGuardNumber(guardNumber);
		GameObject shot = Instantiate(shotPrefab, gunTransform.position, gunTransform.rotation) as GameObject;
		LaserShot laserBehaviour = shot.GetComponent<LaserShot>();
		//Debug.Log(playerPosition);
		laserBehaviour.shotFromGuardNumber = guardNumber;
		laserBehaviour.ShotTarget();
		AudioSource.PlayClipAtPoint(shotClip, transform.position);
	}

	void Shoot () {
		shooting = true;
		playerHealth.TakeDamage(shotDamage);
		movesRecorder.SetKillerGuardNumber(guardNumber);
		//Debug.Log(guardNumber);
		ShotEffects();
	}

	void ShotEffects () {
		laserShotLine.SetPosition(0, laserShotLine.transform.position);
		laserShotLine.SetPosition(1, playerPosition + Vector3.up * 1.5f);
		laserShotLine.enabled = true;
		AudioSource.PlayClipAtPoint(shotClip, transform.position);
	}

	public void PlayLaserEmitter () {
		particles.Play();
		AudioSource.PlayClipAtPoint(chargeClip, transform.position);
	}

	public void StopLaserEmitter () {
		particles.Stop();
	}
}
