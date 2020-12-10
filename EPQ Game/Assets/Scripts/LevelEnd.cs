using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

	private LevelManager levelMangager;
	private ScreenFader fader;
	private GameObject player;

	// Use this for initialization
	void Awake () {
		levelMangager = GameObject.FindObjectOfType<LevelManager>();
		fader = GameObject.FindObjectOfType<ScreenFader>();
		player = GameObject.FindGameObjectWithTag(Tags.player);
	}
	
	void OnTriggerEnter (Collider myCollider) {
		if (myCollider.gameObject == player) {
			fader.FadeOut();
			Invoke("GoToNextLevel", 1.5f);
		}
	}

	void GoToNextLevel () {
		levelMangager.LoadNextLevel();
	}
}
