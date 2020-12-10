using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewPlayer : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject keyPrefab;
	public Vector3 keyPosition;
	public GameObject currentPlayer;
	public bool playerRespawned;

	private PlayerMovementRecorder movesRecorder;

	// Use this for initialization
	void Awake () {
		currentPlayer = GameObject.FindGameObjectWithTag(Tags.player);
		movesRecorder = GetComponent<PlayerMovementRecorder>();
		playerRespawned = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void playerFinishedRespawning () {
		playerRespawned = false;
	}

	public void RespawnPlayer() {
		movesRecorder.numberOfDeaths += 1;
		movesRecorder.reAllocateArrays();
		GameObject respawnedPlayer = Instantiate(playerPrefab,transform.position,transform.rotation) as GameObject;
		GameObject existingKey = GameObject.FindGameObjectWithTag(Tags.key);
		if (existingKey == null) {
			GameObject newKey = Instantiate(keyPrefab, keyPosition, Quaternion.identity) as GameObject;
		}  
		currentPlayer = respawnedPlayer;
		playerRespawned = true;
		Invoke("playerFinishedRespawning", 0.2f);
	}
}
