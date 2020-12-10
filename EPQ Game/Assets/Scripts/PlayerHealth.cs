using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHealth : MonoBehaviour {

	public float health = 100f;
	// Death animation length.
	public float resetAfterDeathTime = 3f;
	public AudioClip teleportClip;

	private Animator anim;
	private HashIDs hashID;
	private ScreenFader fader;
	private float timer;
	private bool playerDead;
	private LastPlayerSighting lastPlayerSighting;
	private bool newPlayerCreated;
	private CreateNewPlayer createNewPlayer;
	private bool fadedOut;
	private LevelManager levelManager;
	private PlayerMovementRecorder movesRecorder;
	private NavMeshAgent nav;
	private bool ragdoll;
	private PlayerInventory inventory;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		anim = GetComponent<Animator>();
		createNewPlayer = GameObject.FindGameObjectWithTag(Tags.cloneController).GetComponent<CreateNewPlayer>();
		movesRecorder = GameObject.FindObjectOfType<PlayerMovementRecorder>();
		hashID = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		fader = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<ScreenFader>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
		timer = 0f;
		newPlayerCreated = false;
		fadedOut = false;
		ragdoll = true;
		inventory = GetComponent<PlayerInventory>();
	}

	void Update () {
		if (health <= 0f) {
			if (!playerDead) {
				playerDead = true;
				//anim.enabled = false;
				fadedOut = true;
			} else if (tag == Tags.player) {
				//LevelReset();
				LevelResetAndReload();
			} else if (tag == Tags.clone) {
				CloneDie();
			}
		}
	}



	public void SwitchNTeleport (GameObject switchGuard) {
		AudioSource.PlayClipAtPoint(teleportClip, transform.position);
		Vector3 playerPos = transform.position;
		//Debug.Log(playerPos);
		Vector3 guardPos = switchGuard.transform.position;
		//Debug.Log(guardPos);
		Vector3 tempPos = new Vector3(50f,50f,50f);
		transform.position = tempPos;
		nav = switchGuard.GetComponent<NavMeshAgent>();
		nav.Warp(playerPos);
		//Debug.Log(switchGuard.name);
		transform.position = guardPos;
		Debug.Log("Teleported");
	}

	void LevelResetAndReload () {
		timer += Time.deltaTime;
		lastPlayerSighting.position = lastPlayerSighting.resetPosition;
		anim.enabled = false;
		if (timer > resetAfterDeathTime+1f) {
			movesRecorder.numberOfDeaths += 1;
			movesRecorder.reAllocateArrays();
			levelManager.ReLoadLevel();
		} else if (timer >= resetAfterDeathTime) {
			fader.FadeOut();
		} else if (ragdoll == true) {
			anim.enabled = false;
			ragdoll = false;
		}
	}

	void LevelReset () {
		timer += Time.deltaTime;
		//Debug.Log("HI");
		//Debug.Log(lastPlayerSighting.position);
		lastPlayerSighting.position = lastPlayerSighting.resetPosition;
		anim.enabled = false;
		if (timer > resetAfterDeathTime+1.5f) {
			fader.FadeIn();
			Debug.Log("old player destroyed");
			DestroyObject(gameObject);
		} else if ((timer > resetAfterDeathTime+1f) && (newPlayerCreated == false)) {
			createNewPlayer.RespawnPlayer();
			newPlayerCreated = true;
			Debug.Log("new player created");
		} else if (timer >= resetAfterDeathTime && fadedOut) {
			fader.FadeOut();
			fadedOut = false;
		}
	}

	public void CloneDie () {
		timer += Time.deltaTime;
		if (timer >= resetAfterDeathTime/2) {
			if (inventory.hasKey == true) {
				movesRecorder.DropKey(transform);
			}
			movesRecorder.clonesAlive -= 1;
			movesRecorder.arrayReplayCount = 0;
			DestroyObject(gameObject);
		}
	}

	void OnTriggerEnter (Collider myTrigger) {
		if (myTrigger.gameObject.tag == Tags.shot) {
			TakeDamage(100f);
		}
	}

	public void TakeDamage (float amount) {
		health -= amount;
	}
}
