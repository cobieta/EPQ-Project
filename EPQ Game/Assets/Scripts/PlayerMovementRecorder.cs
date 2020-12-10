using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRecorder : MonoBehaviour {

	public int minutesRecording = 5;
	public int numberOfClones = 1;
	public bool recordingMoves = false;
	public int clonesAlive = 0;
	public int numberOfDeaths = 0;
	public GameObject clonePrefab, keyPrefab;
	public int arrayReplayCount;

	private GameObject guardToSwitchWith;
	private GameObject clone;
	private float[,,] movesArray;
	private int arrayRecordCount;
	private int arraySize;
	private int[] moveSets;
	private int currentRecordingArray;
	private BasicBehaviour behaviour;
	private Vector3 cloneSpawnPoint;
	private int killerGuardNumber;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);
		arraySize = minutesRecording * 60 * 60;
		movesArray = new float[numberOfClones+1,arraySize,2];
		arrayRecordCount = 0;
		arrayReplayCount = 0;
		currentRecordingArray = 0;
		//Debug.Log(playerStartingPoint);
		//cloneSpawnPoint = playerStartingPoint;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z)) {
			//Debug.Log("Key pressed.");
			CreateClone();
		} 
		if (Input.GetKeyDown(KeyCode.X)) {
			Debug.Log("Destroy clones");
			GameObject[] clones = GameObject.FindGameObjectsWithTag(Tags.clone);
			foreach (GameObject clon in clones) {
				PlayerHealth clonHealth = clon.GetComponent<PlayerHealth>();
				//Debug.Log("Hi");
				//Debug.Log(clon.name);
				clonHealth.health = 0f;
			}
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			// Teleport player.
			guardToSwitchWith = Teleport();
			PlayerHealth playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
			playerHealth.SwitchNTeleport(guardToSwitchWith);
		}
		if (Input.GetKeyDown(KeyCode.V)) {
			// Kill Player
			PlayerHealth playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
			playerHealth.health = 0f;
		}
	}

	public void ChangePosition () {
		Transform playerTransform = GameObject.FindGameObjectWithTag(Tags.player).transform;
		transform.position = playerTransform.position;
		transform.rotation = playerTransform.rotation;
	}

	public void SetKillerGuardNumber (int number) {
		killerGuardNumber = number;
	}

	GameObject Teleport () {
		GameObject[] guards = GameObject.FindGameObjectsWithTag(Tags.enemy);
		foreach (GameObject guard in guards) {
			EnemyShooting shooter = guard.GetComponent<EnemyShooting>();
			if (shooter.guardNumber == killerGuardNumber) {
				//Debug.Log(guard.name);
				return guard;
			}
		}
		return null;
	}

	public void DropKey (Transform transClone) {
		GameObject key = Instantiate(keyPrefab, transClone.position + transform.up, Quaternion.identity);
	}

	// called when player dies.
	public void reAllocateArrays () {
		arrayRecordCount = 0;
		currentRecordingArray++;
		if ((currentRecordingArray % (numberOfClones+1)) == 0) {
			currentRecordingArray = 0;
		}
	}

	// called when player presses Z key from update.
	void CreateClone () {
		if (((clonesAlive+1) <= numberOfClones) && (numberOfDeaths >= (clonesAlive+1))) {
			Debug.Log("clone created");
			clonesAlive++;
			clone = Instantiate(clonePrefab, transform.position, transform.rotation) as GameObject;
			// TODO instatiate clone Prefab, give player transform as parent transform and set individual clone number.
			behaviour = clone.GetComponent<BasicBehaviour>();
			behaviour.cloneNumber = clonesAlive;
		}
	}

	// called every frame from basicbehaviour if character is a clone.
	public float[] ReplayMoves (int cloneNumber) {
		int moveSetNumber = currentRecordingArray - cloneNumber;
		if (currentRecordingArray == 0) {
			moveSetNumber = numberOfClones - cloneNumber;
			moveSetNumber++;
		}
		float h = movesArray[moveSetNumber,arrayReplayCount,0];
		float v = movesArray[moveSetNumber,arrayReplayCount,1];
		arrayReplayCount++;
		//Debug.Log(h);
		//Debug.Log(v);
		float[] thisFrameMove = new float[2] {h,v};
		return thisFrameMove;
	}

	// called every frame from basicbehaviour if character is the player.
	public void RecordMoves (float h, float v) {
		if ((arrayRecordCount < arraySize) && (recordingMoves == true)) {
			movesArray[currentRecordingArray,arrayRecordCount,0] = h;
			movesArray[currentRecordingArray,arrayRecordCount,1] = v;
			arrayRecordCount++;
			//Debug.Log(h);
			//Debug.Log(v);
		}
	}
}
